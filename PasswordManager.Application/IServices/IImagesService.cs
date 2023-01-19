namespace PasswordManager.Application.IServices;

public interface IImagesService
{
    Task<Guid> UploadImage(Stream stream);
    Task<Stream> DownloadImage();
}
