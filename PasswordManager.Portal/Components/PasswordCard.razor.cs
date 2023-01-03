using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Components;

public partial class PasswordCard
{
    [Inject] NavigationManager NavManager { get; set; } = default!;

    [Parameter] public PasswordViewModel Password { get; set; } = null!;
    [Parameter] public EventCallback<PasswordViewModel> OnViewPasswordCredentials { get; set; }

    private string FavoritePasswordMenuText
    {
        get => Password.Favorite.GetValueOrDefault(false) ? "Remove from favorites" : "Add to favorites";
    }

    private string FavoritePasswordMenuIcon
    {
        get => Password.Favorite.GetValueOrDefault(false) ? @Icons.Material.Filled.FavoriteBorder : @Icons.Material.Filled.Favorite;
    }

    private bool MouseOnPasswords { get; set; } = false;

    private bool MouseOnPasswordsBuffer = false;

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
        MouseOnPasswordsBuffer = changeTo;

        if (!changeTo) await Task.Delay(400);

        MouseOnPasswords = MouseOnPasswordsBuffer;
    }

    private async Task ShowPasswordsButtonClicked(MouseEventArgs args)
    {
        await OnViewPasswordCredentials.InvokeAsync(Password);
    }

    private void EditPasswordClicked()
    {
        string url = ApplicationRoutes.Passwords + "/edit/" + Password.Id;

        NavManager.NavigateTo(url);
    }

    private void FavoritesPasswordClicked()
    {
        Password.Favorite = !Password.Favorite.GetValueOrDefault(false);
        StateHasChanged();
    }
}
