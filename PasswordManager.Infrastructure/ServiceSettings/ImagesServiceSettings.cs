using PasswordManager.Shared.Models;

namespace PasswordManager.Infrastructure.ServiceSettings;

internal sealed class ImagesServiceSettings : IServiceSettings
{
    public string PasswordLogoContainerName { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(PasswordLogoContainerName)) { throw new ArgumentNullException(nameof(PasswordLogoContainerName)); }
    }
}
