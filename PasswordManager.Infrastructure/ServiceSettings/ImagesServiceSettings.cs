using PasswordManager.Shared.Models;

namespace PasswordManager.Infrastructure.ServiceSettings;

internal sealed class ImagesServiceSettings : IServiceSettings
{
    public string PasswordLogoContainerName { get; set; } = null!;

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(PasswordLogoContainerName)) { return false; }

        return true;
    }
}
