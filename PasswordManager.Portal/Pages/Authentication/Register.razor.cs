using LanguageExt;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.RegisterPage;

namespace PasswordManager.Portal.Pages.Authentication;

public partial class Register
{
    [Inject] AuthenticationService AuthenticationService { get; set; } = default!;
    [Inject] NavigationManager NavManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;

    private MudForm _mudForm { get; set; }
    private RegisterForm RegisterForm { get; set; } = new();
    private bool RegistrationInProgress = false;

    private async Task RegisterUser()
    {
        


        try
        {
            RegistrationInProgress = true;

            await _mudForm.Validate();

            if (!RegisterForm.IsValid)
            {
                return;
            }

            var result = await AuthenticationService.Register(
                new DtObjects.RegistrationModel(RegisterForm.Email, RegisterForm.OriginalPassword, RegisterForm.FirstName, RegisterForm.LastName));

            result.IfSucc(async u => await SuccessfulRegistration(u));
            result.IfFail(FailedRegistration);
        }
        finally
        {
            RegistrationInProgress = false;
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

        var dialog = DialogService.Show<NotifyDialog>("Success", parameters, options);

        await dialog.Result;

        NavManager.NavigateTo(ApplicationRoutes.Login);
    }

    private void FailedRegistration(Exception ex)
    {
        RegisterForm.ErrorMessage = ex.Message;
    }
}
