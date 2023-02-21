using Blazored.LocalStorage;
using PasswordManager.Portal.Models;
using PasswordManager.Shared.ResponseModels;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PasswordManager.Portal.Services;

public sealed class PasswordLogoService
{
    private readonly ApiClient _apiClient;
    private readonly ISyncLocalStorageService _localStorage;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public PasswordLogoService(ApiClient apiClient, JsonSerializerOptions jsonSerializerOptions, ISyncLocalStorageService localStorage)
    {
        _apiClient = apiClient;
        _jsonSerializerOptions = jsonSerializerOptions;
        _localStorage = localStorage;
    }

    public async Task<LogoModel> GetLogo(Guid? logoId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (logoId is null)
        {
            return new LogoModel()
            {
                FileUrl = "",
                ImageId = Guid.NewGuid(),
                Title = "",
            };
        }

        var logoKey = LogoStorageKey(logoId.Value);

        if (_localStorage.ContainKey(logoKey))
        {
            return _localStorage.GetItem<LogoModel>(logoKey);
        }

        await Task.CompletedTask;

        return new LogoModel()
        {
            FileUrl = logoKey,
            ImageId = logoId.Value,
            Title = logoKey,
        };
    }

    public async Task<string> GetLogoUrl(Guid? logoId, CancellationToken cancellationToken)
    {
        return (await GetLogo(logoId, cancellationToken)).FileUrl;
    }

    public async IAsyncEnumerable<LogoModel> GetAllLogos([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage { Method = HttpMethod.Get };

        var response = await _apiClient.SendAuthorized(request, "/api/Images/GetPasswordLogos", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

        await foreach (var logoImageResponse in JsonSerializer.DeserializeAsyncEnumerable<LogoImageResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken))
        {
            if (logoImageResponse is null)
                continue;

            var logoModel = new LogoModel
            {
                Title = logoImageResponse.Title ?? "",
                FileUrl = logoImageResponse.PublicUrl ?? "",
                ImageId = logoImageResponse.LogoImageId.GetValueOrDefault()
            };

            if (!logoModel.IsValid)
                continue;

            var logoKey = LogoStorageKey(logoModel.ImageId);

            _localStorage.SetItem(logoKey, logoModel);

            yield return logoModel;
        }
    }

    private static string LogoStorageKey(Guid logoId)
    {
        return $"logo_{logoId}";
    }
}
