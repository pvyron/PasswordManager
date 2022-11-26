using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PasswordManager.Portal.Components;

public partial class NotifyDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public string? Message { get; set; }

    void Submit() => MudDialog?.Close(DialogResult.Ok(true));
}
