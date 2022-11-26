using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Logout
{
    [Inject] AuthenticationService _authenticationService { get; set; } = default!;
    [Inject] NavigationManager _navManager { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        _authenticationService.Logout();

        _navManager.NavigateTo(ApplicationRoutes.Index);

        return base.OnInitializedAsync();
    }
}
