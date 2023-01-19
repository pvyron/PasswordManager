using Azure.Storage.Blobs;
using PasswordManager.DataAccess.Interfaces;
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

        await container.UploadBlobAsync(fileName.ToString(), Compress(stream), cancellationToken);

        return fileName;
    }

    public async Task<Stream> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var client = container.GetBlobClient(fileName.ToString());

        var result = await client.DownloadStreamingAsync(cancellationToken: cancellationToken);

        return Decompress(result.Value.Content);
    }

    Stream Compress(Stream stream)
    {
        using var gZipStream = new GZipStream(stream, CompressionMode.Compress);

        return gZipStream;
    }

    Stream Decompress(Stream stream)
    {
        using var gZipStream = new GZipStream(stream, CompressionMode.Decompress);

        return gZipStream;
    }
}
