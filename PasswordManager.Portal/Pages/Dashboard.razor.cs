using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.Dashboard;

namespace PasswordManager.Portal.Pages;

public partial class Dashboard
{
    [Inject] PasswordsService _passwordsService { get; set; } = default!;
    public List<PasswordViewModel> Passwords { get; set; } = new();
    public List<object> Categories { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await _passwordsService.GetPasswordViewModels(CancellationToken.None);

        result.IfSucc(PasswordFetchingSuccess);

        await base.OnInitializedAsync();
    }

    private void PasswordFetchingSuccess(List<PasswordViewModel> passwords)
    {
        Passwords = passwords;
    }
}
