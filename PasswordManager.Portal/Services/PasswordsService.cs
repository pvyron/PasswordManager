using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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

    public async IAsyncEnumerable<PasswordViewModel> GetPasswordViewModels([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage { Method = HttpMethod.Get };

        var response = await _apiClient.SendAuthorized(request, "/api/Passwords", HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

        await foreach (var passwordResponse in JsonSerializer.DeserializeAsyncEnumerable<PasswordResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken))
        {
            if (passwordResponse is null)
                continue;

            yield return new PasswordViewModel
            {
                Id = passwordResponse.Id,
                CategoryId = passwordResponse.CategoryId,
                Description = passwordResponse.Description,
                Password = passwordResponse.Password,
                Title = passwordResponse.Title,
                Username = passwordResponse.Username,
                Favorite = passwordResponse.IsFavorite
            };
        }
    }

    public async Task<Result<PasswordViewModel>> GetPasswordById(string? passwordId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(passwordId))
            {
                return new Result<PasswordViewModel>(new ValueIsNullException("No password found"));
            }

            var response = await _apiClient.GetAuthorized($"/api/Passwords/{passwordId}", cancellationToken);

            var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

                return new Result<PasswordViewModel>(new Exception(errorResponse?.Message));
            }

            var passwordResponse = await JsonSerializer.DeserializeAsync<PasswordResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

            if (passwordResponse is null)
            {
                return new Result<PasswordViewModel>(new ResultIsNullException($"Wrong model {nameof(PasswordResponseModel)} is null"));
            }

            return new PasswordViewModel
            {
                Id = passwordResponse.Id,
                CategoryId = passwordResponse.CategoryId,
                Description = passwordResponse.Description,
                Password = passwordResponse.Password,
                Title = passwordResponse.Title,
                Username = passwordResponse.Username,
                Favorite = passwordResponse.IsFavorite
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordViewModel>(ex);
        }
    }

    public async Task<Result<Unit>> AddNewPassword(NewPassword newPassword, CancellationToken cancellationToken)
    {
        try
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
                    IsFavorite = newPassword.IsFavorite
                },
                typeof(PasswordRequestModel),
                options: _jsonSerializerOptions)
            };

            var response = await _apiClient.SendAuthorized(request, "/api/Passwords", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

                return new Result<Unit>(new Exception(errorResponse?.Message));
            }

            return Unit.Default;
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public async Task<Result<PasswordViewModel>> UpdatePassword(Guid id, NewPassword newPassword, CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = JsonContent.Create(
                new PasswordRequestModel
                {
                    Id = id,
                    CategoryId = newPassword.CategoryId,
                    Description = newPassword.Description,
                    Password = newPassword.Password,
                    Title = newPassword.Title,
                    Username = newPassword.Username,
                    IsFavorite = newPassword.IsFavorite,
                },
                typeof(PasswordRequestModel),
                options: _jsonSerializerOptions)
            };

            var response = await _apiClient.SendAuthorized(request, "/api/Passwords", cancellationToken);

            var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

                return new Result<PasswordViewModel>(new Exception(errorResponse?.Message));
            }

            var passwordResponse = await JsonSerializer.DeserializeAsync<PasswordResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

            if (passwordResponse is null)
            {
                return new Result<PasswordViewModel>(new ResultIsNullException($"Wrong model {nameof(PasswordResponseModel)} is null"));
            }

            return new PasswordViewModel
            {
                Id = passwordResponse.Id,
                CategoryId = passwordResponse.CategoryId,
                Description = passwordResponse.Description,
                Password = passwordResponse.Password,
                Title = passwordResponse.Title,
                Username = passwordResponse.Username,
                Favorite = passwordResponse.IsFavorite,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordViewModel>(ex);
        }
    }

    public async Task<Result<PasswordViewModel>> ChangeFavorability(Guid id, bool isFavorite, CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(isFavorite,
                typeof(bool),
                options: _jsonSerializerOptions)
            };

            var response = await _apiClient.SendAuthorized(request, $"/api/Passwords/{id}", cancellationToken);

            var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

                return new Result<PasswordViewModel>(new Exception(errorResponse?.Message));
            }

            var passwordResponse = await JsonSerializer.DeserializeAsync<PasswordResponseModel>(responseContent, _jsonSerializerOptions, cancellationToken);

            if (passwordResponse is null)
            {
                return new Result<PasswordViewModel>(new ResultIsNullException($"Wrong model {nameof(PasswordResponseModel)} is null"));
            }

            return new PasswordViewModel
            {
                Id = passwordResponse.Id,
                CategoryId = passwordResponse.CategoryId,
                Description = passwordResponse.Description,
                Password = passwordResponse.Password,
                Title = passwordResponse.Title,
                Username = passwordResponse.Username,
                Favorite = passwordResponse.IsFavorite,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordViewModel>(ex);
        }
    }
}
