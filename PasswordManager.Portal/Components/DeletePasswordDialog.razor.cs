using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class DeletePasswordDialog
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public PasswordCardViewModel? Password { get; set; }

    string? Message => $"Are you sure about deleting the password {Password?.Title}?";

    void Submit() => MudDialog?.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}
