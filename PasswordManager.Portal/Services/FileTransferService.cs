using System.Text.Json;

namespace PasswordManager.Portal.Services;

public sealed class FileTransferService
{
    private readonly ApiClient _apiClient;
    private readonly ClientStateData _clientStateData;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public FileTransferService(ApiClient apiClient, ClientStateData clientStateData, JsonSerializerOptions jsonSerializerOptions)
    {
        _apiClient = apiClient;
        _clientStateData = clientStateData;
        _jsonSerializerOptions = jsonSerializerOptions;
    }


}
