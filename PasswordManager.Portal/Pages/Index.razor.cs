using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;

namespace PasswordManager.Portal.Pages;

partial class Index
{
    [Inject] ClientStateData _clientState { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        _clientState.StateHasChanged += _clientState_StateHasChanged;

        return base.OnInitializedAsync();
    }

    private void _clientState_StateHasChanged(object? sender, ClientStateEventArgs e)
    {
        if (e.PropertyChange.Equals(nameof(ClientStateData.IsAuthenticated)))
        {
            StateHasChanged();
        }
    }
}
