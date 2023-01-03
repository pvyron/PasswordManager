using PasswordManager.Shared.Models;

namespace PasswordManager.Infrastructure.ServiceSettings;

internal sealed class AuthorizationServiceSettings : IServiceSettings
{
    public string JwtIssuer { get; set; } = null!;
    public string JwtAudience { get; set; } = null!;
    public string JwtKey { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(JwtIssuer)) { throw new ArgumentNullException(nameof(JwtIssuer)); }
        if (string.IsNullOrWhiteSpace(JwtAudience)) { throw new ArgumentNullException(nameof(JwtAudience)); }
        if (string.IsNullOrWhiteSpace(JwtKey)) { throw new ArgumentNullException(nameof(JwtKey)); }
    }
}
