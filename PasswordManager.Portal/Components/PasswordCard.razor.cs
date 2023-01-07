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
    [Parameter] public EventCallback<PasswordViewModel> OnFavoriteChanged { get; set; }

    private string FavoritePasswordMenuText
    {
        get => Password.Favorite.GetValueOrDefault(false) ? "Remove from favorites" : "Add to favorites";
    }

    private string FavoritePasswordMenuIcon
    {
        get => Password.Favorite.GetValueOrDefault(false) ? @Icons.Material.Filled.FavoriteBorder : @Icons.Material.Filled.Favorite;
    }

    private bool MouseOnPasswords { get; set; } = false;

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

        MouseOnPasswords = _mouseOnPasswordsBuffer;
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

    private async void FavoritesPasswordClicked()
    {
        await OnFavoriteChanged.InvokeAsync(Password);
        StateHasChanged();
    }
}
