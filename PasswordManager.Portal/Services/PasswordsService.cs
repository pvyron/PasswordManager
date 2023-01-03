using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Net.Http.Json;
using System.Text.Json;

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
            var request = new HttpRequestMessage { Method = HttpMethod.Get };

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

    public async Task<Result<Unit>> AddNewPassword(NewPassword newPassword, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = JsonContent.Create(
                new PasswordRequestModel
                {
                    CategoryId = newPassword.CategoryId,
                    Description = newPassword.Description,
                    Password = newPassword.Password,
                    Title = newPassword.Title,
                    Username = newPassword.Username,
                },
                typeof(PasswordRequestModel),
                options: _jsonSerializerOptions)
        };

        var response = await _apiClient.SendAuthorized(request, "/api/Passwords", cancellationToken);

        var responseContent = await response.Content.ReadAsStreamAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions);

            return new Result<Unit>(new Exception(errorResponse?.Message));
        }

        return Unit.Default;
    }
}
