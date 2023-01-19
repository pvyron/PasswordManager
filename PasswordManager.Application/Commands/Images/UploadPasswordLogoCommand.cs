using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.IServices;

namespace PasswordManager.Application.Commands.Images;

public sealed record UploadPasswordLogoCommand(IFormFile File) : IRequest<Result<Guid>>;

public sealed class UploadPasswordLogoCommandHandler : IRequestHandler<UploadPasswordLogoCommand, Result<Guid>>
{
    private readonly IImagesService _imagesService;

    public UploadPasswordLogoCommandHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async Task<Result<Guid>> Handle(UploadPasswordLogoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var memoryStream = new MemoryStream();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var fileId = await _imagesService.UploadImage(memoryStream);

            return fileId;
        }
        catch (Exception ex)
        {
            return new Result<Guid>(ex);
        }
    }
}