using PasswordManager.Shared.Models;

namespace PasswordManager.Infrastructure.ServiceSettings;

internal sealed class AuthorizationServiceSettings : IServiceSettings
{
    public string JwtIssuer { get; set; } = null!;
    public string JwtAudience { get; set; } = null!;
    public string JwtKey { get; set; } = null!;
    public string PublicPasswordHashingSalt { get; set; } = null!;

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(JwtIssuer)) { return false; }
        if (string.IsNullOrWhiteSpace(JwtAudience)) { return false; }
        if (string.IsNullOrWhiteSpace(JwtKey)) { return false; }
        if (string.IsNullOrWhiteSpace(PublicPasswordHashingSalt)) { return false; }

        return true;
    }
}
