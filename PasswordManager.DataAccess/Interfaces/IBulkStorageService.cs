namespace PasswordManager.DataAccess.Interfaces;

public interface IBulkStorageService
{
    Task<Stream> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken);
    Task<Guid> UploadNewFile(string containerName, Stream stream, CancellationToken cancellationToken);
}