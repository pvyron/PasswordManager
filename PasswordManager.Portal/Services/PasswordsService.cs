using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.Pages.Passwords;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PasswordManager.Portal.Services;

public sealed class PasswordsService
{
    private readonly ApiClient _apiClient;
    private readonly ClientStateData _clientStateData;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public PasswordsService(ApiClient apiClient, ClientStateData clientStateData, JsonSerializerOptions jsonSerializerOptions)
    {
        _apiClient = apiClient;
        _clientStateData = clientStateData;
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
                Password = DecryptedPasswordData(passwordResponse.Password),
                Title = passwordResponse.Title,
                Username = DecryptedPasswordData(passwordResponse.Username),
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
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<PasswordViewModel>(new Exception("InternalServerError"));
                }

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
                Password = DecryptedPasswordData(passwordResponse.Password),
                Title = passwordResponse.Title,
                Username = DecryptedPasswordData(passwordResponse.Username),
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
                    Password = EncryptedPasswordData(newPassword.Password),
                    Title = newPassword.Title,
                    Username = EncryptedPasswordData(newPassword.Username),
                    IsFavorite = newPassword.IsFavorite
                },
                typeof(PasswordRequestModel),
                options: _jsonSerializerOptions)
            };

            var response = await _apiClient.SendAuthorized(request, "/api/Passwords", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<Unit>(new Exception("InternalServerError"));
                }

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
                    Password = EncryptedPasswordData(newPassword.Password),
                    Title = newPassword.Title,
                    Username = EncryptedPasswordData(newPassword.Username),
                    IsFavorite = newPassword.IsFavorite,
                },
                typeof(PasswordRequestModel),
                options: _jsonSerializerOptions)
            };

            var response = await _apiClient.SendAuthorized(request, "/api/Passwords", cancellationToken);

            var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<PasswordViewModel>(new Exception("InternalServerError"));
                }

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
                Password = DecryptedPasswordData(passwordResponse.Password),
                Title = passwordResponse.Title,
                Username = DecryptedPasswordData(passwordResponse.Username),
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
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<PasswordViewModel>(new Exception("InternalServerError"));
                }

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
                Password = DecryptedPasswordData(passwordResponse.Password),
                Title = passwordResponse.Title,
                Username = DecryptedPasswordData(passwordResponse.Username),
                Favorite = passwordResponse.IsFavorite,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordViewModel>(ex);
        }
    }

    public async Task<Result<Unit>> DeletePassword(Guid passwordId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete
            };

            var response = await _apiClient.SendAuthorized(request, $"/api/Passwords/{passwordId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<Unit>(new Exception("InternalServerError"));
                }

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

    byte[] EncryptedPasswordData(string password)
    {
        var input = Encoding.UTF8.GetBytes(password);

        var engine = new AesEngine();
        var blockCipher = new CbcBlockCipher(engine);
        var cipher = new PaddedBufferedBlockCipher(blockCipher); // PKCS5/7 padding
        var keyParam = new KeyParameter(_clientStateData.DecryptionToken);

        // Encrypt
        cipher.Init(true, keyParam);
        var encrypted = new byte[cipher.GetOutputSize(input.Length)];
        var length = cipher.ProcessBytes(input, encrypted, 0);
        cipher.DoFinal(encrypted, length);


        return encrypted;
    }

    string DecryptedPasswordData(byte[] encryptedPassword)
    {
        var engine = new AesEngine();
        var blockCipher = new CbcBlockCipher(engine);
        var cipher = new PaddedBufferedBlockCipher(blockCipher); // PKCS5/7 padding
        var keyParam = new KeyParameter(_clientStateData.DecryptionToken);

        // Decrypt
        cipher.Init(false, keyParam);
        var decrypted = new byte[cipher.GetOutputSize(encryptedPassword.Length)];
        var length = cipher.ProcessBytes(encryptedPassword, decrypted, 0);
        cipher.DoFinal(decrypted, length);

        return Encoding.UTF8.GetString(decrypted).TrimEnd('\0');
    }
}
