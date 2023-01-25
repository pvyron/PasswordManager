using LanguageExt.Common;
using Mediator;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.IServices;
using PasswordManager.Shared.ResponseModels;

namespace PasswordManager.Application.Commands.Images;

public sealed record UploadPasswordLogoCommand(IFormFile File, string ImageTitle) : IRequest<Result<LogoImageResponseModel>>;

public sealed class UploadPasswordLogoCommandHandler : IRequestHandler<UploadPasswordLogoCommand, Result<LogoImageResponseModel>>
{
    private readonly IImagesService _imagesService;

    public UploadPasswordLogoCommandHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async ValueTask<Result<LogoImageResponseModel>> Handle(UploadPasswordLogoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var extension = Path.GetExtension(request.File.FileName);

            using var memoryStream = new MemoryStream();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var uploadedImage = await _imagesService.UploadImage(memoryStream, request.ImageTitle, extension, cancellationToken);

            return new LogoImageResponseModel
            {
                LogoImageId = uploadedImage.LogoId,
                Title = uploadedImage.Title,
                PublicUrl = uploadedImage.FileUrl
            };
        }
        catch (Exception ex)
        {
            return new Result<LogoImageResponseModel>(ex);
        }
    }
}