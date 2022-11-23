using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Shared;

public partial class MainLayout
{
    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }




    public async Task Login()
    {
        await _authenticationStateService.Login();
    }

    public async Task Logout()
    {
        await _authenticationStateService.Logout();
    }
}