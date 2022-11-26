using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Pages;

partial class Index
{
    [Inject] ClientStateData _clientState { get; set; } = default!;
    [Inject] NavigationManager _navigationManager { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        _clientState.StateHasChanged += _clientState_StateHasChanged;

        if (_clientState.IsAuthenticated)
            _navigationManager.NavigateTo(ApplicationRoutes.Dashboard);
        else if (!_clientState.IsAuthenticated)
            _navigationManager.NavigateTo(ApplicationRoutes.Login);

        return base.OnInitializedAsync();
    }

    private void _clientState_StateHasChanged(object? sender, ClientStateEventArgs e)
    {
        if (!e.PropertyChanged.Equals(nameof(ClientStateData.IsAuthenticated))) return;

        StateHasChanged();
    }
}
