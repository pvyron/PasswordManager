using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.RegisterPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Register
{
    [Inject]
    private AuthenticationService _authenticationStateService { get; set; } = default!;

    private RegisterForm _registerForm = new();
    private bool _registrationInProgress = false;

    private async Task RegisterUser()
    {
        try
        {
            _registrationInProgress = true;


            await Task.Delay(1500);
        }
        finally
        {
            _registrationInProgress = false;
        }
    }
}
