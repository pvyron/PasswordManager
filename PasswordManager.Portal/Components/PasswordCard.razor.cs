using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class PasswordCard
{
    [Parameter] public PasswordViewModel Password { get; set; } = null!;
    [Parameter] public EventCallback<PasswordViewModel> OnViewPasswordCredentials { get; set; }

    MudCard? _passwordCard { get; set; }
    private bool _mouseOnPasswords { get; set; } = false;

    private bool _mouseOnPasswordsBuffer = false;

    private async Task OnPasswordsMouseOver()
    {
        await ChangeMouseOnPasswordsValue(true);
    }

    private async Task OnPasswordMouseOut()
    {
        await ChangeMouseOnPasswordsValue(false);
    }

    private async Task ChangeMouseOnPasswordsValue(bool changeTo)
    {
        _mouseOnPasswordsBuffer = changeTo;

        if (!changeTo) await Task.Delay(400);

        _mouseOnPasswords = _mouseOnPasswordsBuffer;
    }

    private async Task ShowPasswordsButtonClicked(MouseEventArgs args)
    {
        await OnViewPasswordCredentials.InvokeAsync(Password);
    }
}
