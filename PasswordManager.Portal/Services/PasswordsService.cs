using LanguageExt.Common;
using LanguageExt;
using LanguageExt.Pipes;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;

namespace PasswordManager.Portal.Services;

public sealed class PasswordsService
{
    private readonly ApiClient _apiClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public PasswordsService(ApiClient apiClient, JsonSerializerOptions jsonSerializerOptions)
    {
        _apiClient = apiClient;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<Result<List<PasswordViewModel>>> GetPasswordViewModels(CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };

            var response = await _apiClient.GetAuthorized("/api/Passwords", cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStreamAsync();

            var responseModel = await JsonSerializer.DeserializeAsync<List<PasswordResponseModel>>(responseContent, _jsonSerializerOptions);

            if (responseModel is null)
            {
                return new Result<List<PasswordViewModel>>(new Exception($"Wrong model {nameof(PasswordResponseModel)} is null"));
            }

            var passwordViewModels = responseModel.Select(r => new PasswordViewModel
            {
                Description = r.Description,
                Password = r.Password,
                Title = r.Title,
                Username = r.Username,
            }).ToList();

            return passwordViewModels;
        }
        catch (Exception ex)
        {
            return new Result<List<PasswordViewModel>>(ex);
        }
    }
}
