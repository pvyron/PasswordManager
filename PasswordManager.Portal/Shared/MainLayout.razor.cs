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
}