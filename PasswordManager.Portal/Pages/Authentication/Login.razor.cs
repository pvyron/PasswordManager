using LanguageExt;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.LoginPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Login
{
    [Inject] AuthenticationService _authenticationStateService { get; set; } = default!;
    [Inject] NavigationManager _navManager { get; set; } = default!;

    private LoginForm _loginForm { get; set; } = new();
    private bool _authenticationInProgress { get; set; } = false;

    private async Task LoginUser()
    {
        try
        {
            _authenticationInProgress = true;

            _loginForm.ErrorMessage = "";

            if (!_loginForm.IsValid)
                return;

            var result = await _authenticationStateService.Login(new DtObjects.LoginModel(_loginForm.Email!, _loginForm.Password!));

            result.IfSucc(SuccessfulLogin);

            result.IfFail(FailedLogin);
        }
        finally
        {
            _authenticationInProgress = false;
        }
    }

    private void SuccessfulLogin(Unit unit)
    {
        _navManager.NavigateTo(ApplicationRoutes.Index);
    }

    private void FailedLogin(Exception ex) 
    {
        _loginForm.ErrorMessage = ex.Message;
    }
}
