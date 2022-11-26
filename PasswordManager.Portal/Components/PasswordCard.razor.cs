using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class PasswordCard
{
    [Parameter] public PasswordViewModel Password { get; set; } = null!;
}
