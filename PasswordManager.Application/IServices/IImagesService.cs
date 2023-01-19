namespace PasswordManager.Application.IServices;

public interface IImagesService
{
    Task<Guid> UploadImage(Stream stream, CancellationToken cancellationToken);
    Task<Stream> DownloadImage(Guid imageId, CancellationToken cancellationToken);
}
