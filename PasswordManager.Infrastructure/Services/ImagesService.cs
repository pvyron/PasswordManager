using Microsoft.Extensions.Options;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Infrastructure.ServiceSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

internal sealed class ImagesService : IImagesService
{
    private readonly IBulkStorageService _bulkStorageService;
    private readonly ISqlDbContext _sqlDbContext;
    private readonly ImagesServiceSettings _settings;

    public ImagesService(IOptions<ImagesServiceSettings> options, IBulkStorageService bulkStorageService, ISqlDbContext sqlDbContext)
    {
        _settings = options.Value;
        _settings.Validate();

        _bulkStorageService = bulkStorageService;
        _sqlDbContext = sqlDbContext;
    }

    public async Task<Stream> DownloadImage(Guid imageId, CancellationToken cancellationToken)
    {
        return await _bulkStorageService.DownloadFile(_settings.PasswordLogoContainerName, imageId, cancellationToken);
    }

    public async Task<Guid> UploadImage(Stream stream, CancellationToken cancellationToken)
    {
        return await _bulkStorageService.UploadNewFile(_settings.PasswordLogoContainerName, stream, cancellationToken);
    }
}
