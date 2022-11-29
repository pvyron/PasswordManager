using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class PasswordCard
{
    [Inject] IDialogService _dialogService { get; set; } = default!;
    [Parameter] public PasswordViewModel Password { get; set; } = null!;
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
        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.ExtraLarge
        };

        var parameters = new DialogParameters
        {
            { "Username", Password.Username },
            { "Password", Password.Password }
        };

        var dialog = _dialogService.Show<PasswordCredentialsDialog>(Password.Title, parameters, options);

        await dialog.Result;
    }
}
