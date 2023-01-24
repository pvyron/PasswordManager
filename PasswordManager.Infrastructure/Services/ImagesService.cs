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
using System.Runtime.InteropServices;

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

        img.Size().Deconstruct(out int originalWidth, out int originalHeight);
        var ratio = decimal.Divide(originalWidth, originalHeight);

        img.Mutate(i => i.Resize((int)Math.Round(200 * ratio, 0, MidpointRounding.AwayFromZero), 200));

        using var jpegStream = new MemoryStream();
        await img.SaveAsync(jpegStream, _imageManipulationService.ImageEncoder, cancellationToken);

        var uploadedImage = await _bulkStorageService.UploadNewFile(_settings.PasswordLogoContainerName, jpegStream, cancellationToken);

        img.Mutate(i => i.Resize((int)Math.Round(25 * ratio, 0, MidpointRounding.AwayFromZero), 25));
        await img.SaveAsync(jpegStream, _imageManipulationService.ImageEncoder, cancellationToken);

        var uploadedThumbnail = await _bulkStorageService.UploadNewFile(_settings.PasswordLogoContainerName, jpegStream, cancellationToken);

        var logoDbModelResult = await _sqlDbContext.PasswordLogos.AddAsync(new PasswordLogoDbModel
        {
            BulkStorageImageName = uploadedImage.FileName,
            BulkStorageThumbnailName = uploadedThumbnail.FileName,
            ImageUrl = uploadedImage.PublicUrl,
            ThumbnailUrl = uploadedThumbnail.PublicUrl,
            Title = imageName
        }, cancellationToken);

        await _sqlDbContext.SaveChangesAsync(cancellationToken);

        var logoDbModel = logoDbModelResult.Entity;

        return new PasswordLogoModel
        {
            Title = logoDbModel.Title,
            FileUrl = logoDbModel.ImageUrl,
            FileExtension = "jpg",
            ThumbnailUrl = logoDbModel.ThumbnailUrl,
            ThumbnailExtension = "jpg"
        };
    }

    public async IAsyncEnumerable<PasswordLogoModel> GetAllPasswordLogos([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var logoDbModel in _sqlDbContext.PasswordLogos.ToAsyncEnumerable().ConfigureAwait(true).WithCancellation(cancellationToken))
        {
            yield return new PasswordLogoModel
            {
                FileExtension = "jpg",
                FileUrl = logoDbModel.ImageUrl,
                ThumbnailExtension = "jpg",
                ThumbnailUrl = logoDbModel.ThumbnailUrl,
                Title = logoDbModel.Title
            };
        }
    }
}
