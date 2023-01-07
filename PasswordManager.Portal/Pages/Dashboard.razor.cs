using LanguageExt;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Pages;

public partial class Dashboard
{
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] PasswordsService PasswordsService { get; set; } = default!;
    public List<PasswordViewModel> Passwords { get; set; } = new();
    public List<object> Categories { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await FetchPasswords();

        await base.OnInitializedAsync();
    }

    private async Task FetchPasswords()
    {
        Passwords = new();

        await foreach (var password in PasswordsService.GetPasswordViewModels(CancellationToken.None))
        {
            Passwords.Add(password);
            StateHasChanged();
        }
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

        var dialog = DialogService.Show<PasswordCredentialsDialog>(passwordViewModel.Title, parameters, options);

        await dialog.Result;
    }

    private async Task OnFavoriteChanged(PasswordViewModel passwordViewModel)
    {
        var updatedPasswordResult = await PasswordsService.ChangeFavorability(passwordViewModel.Id.GetValueOrDefault(Guid.NewGuid()), !passwordViewModel.Favorite.GetValueOrDefault(false), CancellationToken.None);

        updatedPasswordResult.IfSucc(
            pm =>
            {
                var outedatedPassword = Passwords.FirstOrDefault(pvm => pvm.Id == pm.Id);

                if (outedatedPassword is null)
                    return;

                Passwords.Remove(outedatedPassword);

                Passwords.Add(pm);
            });
        updatedPasswordResult.IfFail(ex => Console.WriteLine(ex.Message));
    }
}
