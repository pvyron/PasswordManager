using Azure.Storage.Blobs;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.DataAccess.StorageModels;

namespace PasswordManager.DataAccess.Implementations;

internal sealed class BulkStorageService : IBulkStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BulkStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<UploadedFileModel> UploadNewFile(string containerName, Stream stream, CancellationToken cancellationToken)
    {
        var fileName = Guid.NewGuid().ToString();

        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var blob = container.GetBlobClient(fileName);

        stream.Position = 0;

        await blob.UploadAsync(stream, cancellationToken);

        return new UploadedFileModel
        {
            FileName = fileName,
            PublicUrl = blob.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.MaxValue).AbsoluteUri
        };
    }

    public async Task<byte[]> DownloadFile(string containerName, Guid fileName, CancellationToken cancellationToken)
    {
        var compressedStream = await DownloadFileAsStream(containerName, fileName, cancellationToken) as MemoryStream;

        try
        {
            compressedStream!.Position = 0;

            return compressedStream.ToArray();
        }
        finally
        {
            compressedStream?.Dispose();
        }
    }

    public async Task<Stream> DownloadFileAsStream(string containerName, Guid fileName, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var client = container.GetBlobClient(fileName.ToString());

        var compressedStream = new MemoryStream();

        await client.DownloadToAsync(compressedStream, cancellationToken);

        return compressedStream;
    }
}
