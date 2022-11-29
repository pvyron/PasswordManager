using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Shared;

public partial class MainLayout
{
    bool _drawerOpen = false;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}