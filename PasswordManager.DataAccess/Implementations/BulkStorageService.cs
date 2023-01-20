using Azure.Storage.Blobs;
using PasswordManager.DataAccess.Interfaces;
using System.IO;
using System.IO.Compression;

namespace PasswordManager.DataAccess.Implementations;

internal sealed class BulkStorageService : IBulkStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BulkStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<Guid> UploadNewFile(string containerName, Stream stream, CancellationToken cancellationToken)
    {
        var fileName = Guid.NewGuid();

        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        
        await container.UploadBlobAsync(fileName.ToString(), stream, cancellationToken);

        return fileName;
    }

    public async Task<byte[]> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var client = container.GetBlobClient(fileName.ToString());

        using var compressedStream = new MemoryStream();

        await client.DownloadToAsync(compressedStream, cancellationToken);

        return compressedStream.ToArray();
    }
}
