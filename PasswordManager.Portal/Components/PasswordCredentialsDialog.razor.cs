using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class PasswordCredentialsDialog
{
    [Inject] ClipboardService _clipboardService { get; set; } = default!;
    [Inject] ISnackbar _snackbarService { get; set; } = default!;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public PasswordCardViewModel? Password { get; set; } = default!;

    void OkButtonClicked() => MudDialog?.Close(DialogResult.Ok(true));

    private async Task CopyUsername()
    {
        await _clipboardService.CopyToClipboard(Password?.Username);
        CopiedToClipboard("Username", Severity.Info);
    }

    private async Task CopyPassword()
    {
        await _clipboardService.CopyToClipboard(Password?.Password);
        CopiedToClipboard("Password", Severity.Info);
    }

    void CopiedToClipboard(string copiedValue, Severity severity)
    {
        var snackbar = _snackbarService.Add($"{copiedValue} is copied to clipboard", severity, config =>
        {
            config.Icon = GlobalIcons.ClipboardIcon;
        });
    }
}
