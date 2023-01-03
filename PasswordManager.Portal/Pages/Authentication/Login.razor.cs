using LanguageExt;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.LoginPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Login
{
    [Inject] AuthenticationService AuthenticationService { get; set; } = default!;
    [Inject] NavigationManager NavManager { get; set; } = default!;

    private LoginForm LoginForm { get; set; } = new();
    private bool AuthenticationInProgress { get; set; } = false;

    private async Task LoginUser()
    {
        try
        {
            AuthenticationInProgress = true;

            LoginForm.ErrorMessage = "";

            if (!LoginForm.IsValid)
                return;

            var result = await AuthenticationService.Login(new DtObjects.LoginModel(LoginForm.Email!, LoginForm.Password!, LoginForm.RememberMe));

            result.IfSucc(SuccessfulLogin);

            result.IfFail(FailedLogin);
        }
        finally
        {
            AuthenticationInProgress = false;
        }
    }

    private async Task PasswordFieldOnKeyPress(KeyboardEventArgs args)
    {
        if (!args.Key.Equals("Enter")) return;

        await LoginUser();
    }

    private void SuccessfulLogin(Unit unit)
    {
        NavManager.NavigateTo(ApplicationRoutes.Index);
    }

    private void FailedLogin(Exception ex)
    {
        LoginForm.ErrorMessage = ex.Message;
    }
}
