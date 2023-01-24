using PasswordManager.Domain.Models;

namespace PasswordManager.Application.IServices;

public interface IImagesService
{
    Task<PasswordLogoModel> UploadImage(Stream stream, string imageName, string fileExtension, CancellationToken cancellationToken);
    Task<byte[]> DownloadImage(Guid imageId, CancellationToken cancellationToken);
    Task<string> DownloadImageInBase64(Guid imageId, CancellationToken cancellationToken);
    IAsyncEnumerable<PasswordLogoModel> GetAllPasswordLogos(CancellationToken cancellationToken);
}
