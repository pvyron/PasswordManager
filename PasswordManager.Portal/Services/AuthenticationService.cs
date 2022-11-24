using PasswordManager.Portal.DtObjects;

namespace PasswordManager.Portal.Services;

public sealed class AuthenticationService
{
    private readonly ClientStateData _clientStateData;
    private readonly ApiClient _apiClient;

	public AuthenticationService(ClientStateData clientStateData, ApiClient apiClient)
	{
		_clientStateData = clientStateData;
		_apiClient = apiClient;
	}

	public async Task<bool> Register(RegistrationModel registrationModel)
	{
        await Task.Delay(1000);

		return true;
    }

	public async Task Login(LoginModel loginModel)
	{
		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Get
		};

		request.Headers.Add("email", loginModel.Email);
        request.Headers.Add("password", loginModel.Password);

        var response = await _apiClient.SendAnonymous(request, "/api/Authorization/Login", CancellationToken.None);
    }

	public async Task Logout()
	{
		await Task.Delay(500);

	}
}
