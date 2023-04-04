using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.EditPassword;
using PasswordManager.Portal.ViewModels.PasswordData;

namespace PasswordManager.Portal.Components;

public partial class PasswordDataForm
{
    [Inject] PasswordsService _passwordsService { get; set; } = default!;
    [Inject] PasswordLogoService _passwordLogoService { get; set; } = default!;

    private PasswordDataFormViewModel _viewModel = new();

    bool SaveButtonDisabled => FormComponentsDisabled;// || !EditPasswordForm.IsPasswordChanged;
    bool FormComponentsDisabled => PasswordFetchingInProgress && SavingPasswordInProgress;
    bool SavingPasswordInProgress { get; set; } = false;
    bool PasswordFetchingInProgress { get; set; } = false;
    bool IsDrawerOpen { get; set; } = false;
}
