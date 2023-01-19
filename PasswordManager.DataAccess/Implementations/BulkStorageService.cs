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

        using var gZipStream = new GZipStream(stream, CompressionMode.Compress);
        await container.UploadBlobAsync(fileName.ToString(), gZipStream, cancellationToken);

        return fileName;
    }

    public async Task<Stream> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var client = container.GetBlobClient(fileName.ToString());

        var result = await client.DownloadStreamingAsync(cancellationToken: cancellationToken);
        using var gZipStream = new GZipStream(result.Value.Content, CompressionMode.Decompress);

        return gZipStream;
    }
}
