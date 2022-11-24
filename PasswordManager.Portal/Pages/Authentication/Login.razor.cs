using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.LoginPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Login
{
    [Inject]
    private AuthenticationService _authenticationStateService { get; set; } = default!;

    private LoginForm _loginForm { get; set; } = new();
    private bool _authenticationInProgress { get; set; } = false;

    private async Task LoginUser()
    {
        try
        {
            _authenticationInProgress = true;

            await _authenticationStateService.Login(new DtObjects.LoginModel(_loginForm.Email, _loginForm.Password));
        }
        finally
        {
            _authenticationInProgress = false;
        }
    }
}
