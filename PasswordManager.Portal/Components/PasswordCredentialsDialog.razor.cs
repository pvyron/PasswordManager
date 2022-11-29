using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Components;

public partial class PasswordCredentialsDialog
{
    [Inject] ClipboardService _clipboardService { get; set; } = default!;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public string? Username { get; set; }
    [Parameter] public string? Password { get; set; }

    void OkButtonClicked() => MudDialog?.Close(DialogResult.Ok(true));

    private async Task CopyUsername()
    {
        await _clipboardService.CopyToClipboard(Username);
    }

    private async Task CopyPassword()
    {
        await _clipboardService.CopyToClipboard(Password);
    }
}
