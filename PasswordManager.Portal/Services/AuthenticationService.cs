using LanguageExt;
using LanguageExt.Common;
using MudBlazor.Utilities;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.Models;
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

	public async Task<bool> Register(RegistrationModel registrationModel)
	{
        await Task.Delay(1000);

		return true;
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

			if (!response.IsSuccessStatusCode)
			{
				return new Result<Unit>(new Exception(response.ReasonPhrase));
			}

			var responseContent = await response.Content.ReadAsStreamAsync();

			var responseModel = await JsonSerializer.DeserializeAsync<LoginReponseModel>(responseContent, _jsonSerializerOptions);

			if (responseModel is null)
			{
				return new Result<Unit>(new Exception($"Wrong model {nameof(LoginReponseModel)} is null"));
			}

			var user = new User
			{
				AccessToken = responseModel.AccessToken,
				Email = responseModel.Email,
				FirstName = responseModel.FirstName,
				LastName = responseModel.LastName,
			};

			_clientStateData.LoggedIn(user);

			return Unit.Default;
		}
		catch (Exception ex)
		{
			return new Result<Unit>(ex);
        }
	}

	public async Task Logout()
	{
		await Task.Delay(500);

	}
}
