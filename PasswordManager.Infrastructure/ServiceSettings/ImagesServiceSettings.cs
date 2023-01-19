using PasswordManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.ServiceSettings;

internal sealed class ImagesServiceSettings : IServiceSettings
{
    public string PasswordLogoContainerName { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(PasswordLogoContainerName)) { throw new ArgumentNullException(nameof(PasswordLogoContainerName)); }
    }
}
