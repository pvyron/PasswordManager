using PasswordManager.DataAccess.StorageModels;

namespace PasswordManager.DataAccess.Interfaces;

public interface IBulkStorageService
{
    Task<byte[]> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken);
    Task<Stream> DownloadFileAsStream(string containerName, Guid fileName, CancellationToken cancellationToken);
    Task<UploadedFileModel> UploadNewFile(string containerName, Stream stream, CancellationToken cancellationToken);
}