using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.IServices;
using PasswordManager.Shared.ResponseModels;

namespace PasswordManager.Application.Commands.Images;

public sealed record UploadPasswordLogoCommand(IFormFile File, string ImageTitle) : IRequest<Result<ImageLogoResponseModel>>;

public sealed class UploadPasswordLogoCommandHandler : IRequestHandler<UploadPasswordLogoCommand, Result<ImageLogoResponseModel>>
{
    private readonly IImagesService _imagesService;

    public UploadPasswordLogoCommandHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async Task<Result<ImageLogoResponseModel>> Handle(UploadPasswordLogoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var extension = Path.GetExtension(request.File.FileName);

            using var memoryStream = new MemoryStream();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var uploadedImage = await _imagesService.UploadImage(memoryStream, request.ImageTitle, extension, cancellationToken);

            return new ImageLogoResponseModel
            {
                PublicUrl = uploadedImage.FileUrl,
                ThumbnailUrl = uploadedImage.ThuumbnailUrl
            };
        }
        catch (Exception ex)
        {
            return new Result<ImageLogoResponseModel>(ex);
        }
    }
}