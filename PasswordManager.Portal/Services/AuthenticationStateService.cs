namespace PasswordManager.Portal.Services;

public sealed class AuthenticationStateService
{
	public bool IsAuthenticated { get; private set; }

	public AuthenticationStateService()
	{
		IsAuthenticated = false;
    }

	public async Task Login()
	{
		await Task.Delay(1000);

		IsAuthenticated = true;
    }

	public async Task Logout()
	{
		await Task.Delay(500);

		IsAuthenticated = false;
	}
}
