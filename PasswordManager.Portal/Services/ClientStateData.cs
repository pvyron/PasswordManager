using System.Security.Claims;

namespace PasswordManager.Portal.Services;

public sealed class ClientStateData
{
    public bool IsAuthenticated { get; private set; }
    public string? AccessToken { get; private set; }

    public ClientStateData()
    {
        IsAuthenticated = false;
    }

    public void LoggedIn(IEnumerable<Claim> claims, string accessToken)
    {
        AccessToken = accessToken;
        IsAuthenticated = true;
    }
}
