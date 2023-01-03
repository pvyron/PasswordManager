using LanguageExt;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.LoginPage;
using PasswordManager.Portal.ViewModels.RegisterPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Register
{
    [Inject] AuthenticationService _authenticationService { get; set; } = default!;
    [Inject] NavigationManager _navManager { get; set; } = default!;
    [Inject] IDialogService _dialogService { get; set; } = default!;

    private RegisterForm _registerForm = new();
    private bool _registrationInProgress = false;

    private async Task RegisterUser()
    {
        try
        {
            _registrationInProgress = true;

            if (!_registerForm.IsValid)
            {
                return;
            }

            var result = await _authenticationService.Register(
                new DtObjects.RegistrationModel(_registerForm.Email, _registerForm.OriginalPassword, _registerForm.FirstName, _registerForm.LastName));

            result.IfSucc(async u => await SuccessfulRegistration(u));
            result.IfFail(FailedRegistration);
        }
        finally
        {
            _registrationInProgress = false;
        }
    }

    private async Task SuccessfulRegistration(Unit unit)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters
        {
            { "Message", "Registration was successfull, please login" }
        };

        var dialog = _dialogService.Show<NotifyDialog>("Success", parameters, options);

        await dialog.Result;

        _navManager.NavigateTo(ApplicationRoutes.Login);
    }

    private void FailedRegistration(Exception ex)
    {
        _registerForm.ErrorMessage = ex.Message;
    }
}
