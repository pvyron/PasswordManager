using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Domain.Models;
using PasswordManager.Infrastructure.ServiceSettings;
using PasswordManager.Infrastructure.ToolServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.Services;

internal sealed class ImagesService : IImagesService
{
    private readonly ImageManipulationService _imageManipulationService;
    private readonly IBulkStorageService _bulkStorageService;
    private readonly ISqlDbContext _sqlDbContext;
    private readonly ImagesServiceSettings _settings;

    public ImagesService(IOptions<ImagesServiceSettings> options, ImageManipulationService imageManipulationService, IBulkStorageService bulkStorageService, ISqlDbContext sqlDbContext)
    {
        _settings = options.Value;
        _settings.Validate();

        _imageManipulationService = imageManipulationService;
        _bulkStorageService = bulkStorageService;
        _sqlDbContext = sqlDbContext;
    }

    public async Task<string> DownloadImageInBase64(Guid imageId, CancellationToken cancellationToken)
    {
        //var logo = await _sqlDbContext.PasswordLogos.FindAsync(imageId);

        var stream = await _bulkStorageService.DownloadFileAsStream(_settings.PasswordLogoContainerName, imageId, cancellationToken);

        try
        {
            using var img = await Image.LoadAsync(stream, _imageManipulationService.ImageDecoder, cancellationToken);

            return img.ToBase64String(_imageManipulationService.ImageFormat);
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }

    public async Task<byte[]> DownloadImage(Guid imageId, CancellationToken cancellationToken)
    {
        return await _bulkStorageService.DownloadFile(_settings.PasswordLogoContainerName, imageId, cancellationToken);
    }

    public async Task<PasswordLogoModel> UploadImage(Stream stream, string imageName, string fileExtension, CancellationToken cancellationToken)
    {
        var decoder = _imageManipulationService.GetImageDecoder(fileExtension);

        stream.Position = 0;

        using var img = await Image.LoadAsync(stream, decoder, cancellationToken);

        using var jpegStream = new MemoryStream();
        await img.SaveAsync(jpegStream, _imageManipulationService.ImageEncoder, cancellationToken);

        var uploadedImage = await _bulkStorageService.UploadNewFile(_settings.PasswordLogoContainerName, jpegStream, cancellationToken);

        var logoDbModelResult = await _sqlDbContext.PasswordLogos.AddAsync(new PasswordLogoDbModel
        {
            BulkStorageImageName = uploadedImage.FileName,
            ImageUrl = uploadedImage.PublicUrl,
            Title = imageName
        }, cancellationToken);

        await _sqlDbContext.SaveChangesAsync(cancellationToken);

        var logoDbModel = logoDbModelResult.Entity;

        return new PasswordLogoModel
        {
            LogoId = logoDbModel.Id,
            Title = logoDbModel.Title,
            FileUrl = logoDbModel.ImageUrl,
            FileExtension = "jpg",
        };
    }

    public async IAsyncEnumerable<PasswordLogoModel> GetAllPasswordLogos([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var logoDbModel in _sqlDbContext.PasswordLogos.ToAsyncEnumerable().ConfigureAwait(true).WithCancellation(cancellationToken))
        {
            yield return new PasswordLogoModel
            {
                LogoId = logoDbModel.Id,
                FileExtension = "jpg",
                FileUrl = logoDbModel.ImageUrl,
                Title = logoDbModel.Title
            };
        }
    }
}
