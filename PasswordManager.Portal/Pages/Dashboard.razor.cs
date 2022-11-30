using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Pages;

public partial class Dashboard
{
    [Inject] IDialogService _dialogService { get; set; } = default!;
    [Inject] PasswordsService _passwordsService { get; set; } = default!;
    public List<PasswordViewModel> Passwords { get; set; } = new();
    public List<object> Categories { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await _passwordsService.GetPasswordViewModels(CancellationToken.None);

        result.IfSucc(PasswordFetchingSuccess);

        await base.OnInitializedAsync();
    }

    private void PasswordFetchingSuccess(List<PasswordViewModel> passwords)
    {
        Passwords = passwords;
    }

    private async Task OnViewPasswordCredentials(PasswordViewModel passwordViewModel)
    {
        var options = new DialogOptions
        {
            CloseButton = false,
            CloseOnEscapeKey = true,
            DisableBackdropClick = false,
            FullWidth = true,
            FullScreen = false,
            NoHeader = false,
            Position = DialogPosition.Center
        };

        var parameters = new DialogParameters
        {
            {"Password", passwordViewModel }
        };

        var dialog = _dialogService.Show<PasswordCredentialsDialog>(passwordViewModel.Title, parameters, options);

        await dialog.Result;
    }
}
