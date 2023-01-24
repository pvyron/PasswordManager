using LanguageExt;
using LanguageExt.Common;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.Models;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PasswordManager.Portal.Services;

public sealed class AuthenticationService
{
    private readonly ClientStateData _clientStateData;
    private readonly ApiClient _apiClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AuthenticationService(ClientStateData clientStateData, ApiClient apiClient, JsonSerializerOptions jsonSerializerOptions)
    {
        _clientStateData = clientStateData;
        _apiClient = apiClient;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<Result<Unit>> Register(RegistrationModel registrationModel)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Content = JsonContent.Create(new RegistrationRequestModel
                {
                    Email = registrationModel.Email,
                    FirstName = registrationModel.FirstName,
                    LastName = registrationModel.LastName,
                    Password = registrationModel.Password,
                }),
                Method = HttpMethod.Post
            };

            var response = await _apiClient.SendAnonymous(request, "/api/Authorization/Register", CancellationToken.None);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<Unit>(new Exception("InternalServerError"));
                }

                var responseContent = await response.Content.ReadAsStreamAsync();

                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions);

                return new Result<Unit>(new Exception(errorResponse?.Message));
            }

            return Unit.Default;
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public async Task<Result<Unit>> Login(LoginModel loginModel)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };

            request.Headers.Add("email", loginModel.Email);
            request.Headers.Add("password", loginModel.Password);

            var response = await _apiClient.SendAnonymous(request, "/api/Authorization/Login", CancellationToken.None);

            var responseContent = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return new Result<Unit>(new Exception("InternalServerError"));
                }

                var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponseModel>(responseContent, _jsonSerializerOptions);

                return new Result<Unit>(new Exception(errorResponse?.Message));
            }

            var responseModel = await JsonSerializer.DeserializeAsync<LoginReponseModel>(responseContent, _jsonSerializerOptions);

            if (responseModel is null)
            {
                return new Result<Unit>(new ResultIsNullException($"Wrong model {nameof(LoginReponseModel)} is null"));
            }

            var decryptionToken = SHA256.HashData(Encoding.UTF8.GetBytes(loginModel.Password));

            var user = new UserModel
            {
                AccessToken = responseModel.AccessToken,
                Email = responseModel.Email,
                FirstName = responseModel.FirstName,
                LastName = responseModel.LastName,
                RemainLoggedIn = loginModel.RememberMe,
                DecryptionToken = decryptionToken
            };

            _clientStateData.LoggedIn(user);

            return Unit.Default;
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public Task Logout()
    {
        _clientStateData.Logout();
        return Task.CompletedTask;
    }
}
