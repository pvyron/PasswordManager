using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Shared;

public partial class NavMenu
{
    [Inject] ClientStateData _clientState { get; set; } = default!;
    private bool _isMyPasswordsExpanded { get; set; } = false;

    protected override Task OnInitializedAsync()
    {
        _clientState.StateHasChanged += _clientState_StateHasChanged;

        return base.OnInitializedAsync();
    }

    private void _clientState_StateHasChanged(object? sender, ClientStateEventArgs e)
    {
        if (!e.PropertyChanged.Equals(nameof(ClientStateData.IsAuthenticated))) return;

        StateHasChanged();
    }
}
