using MudBlazor;
using System.Net.Http.Json;
using PasswordManager.Portal.ViewModels.Dashboard;
using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.ViewPasswords;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class ViewPasswords
{
    [Inject] PasswordsService _passwordsService { get; set; } = default!;

    List<PasswordRowViewModel> Passwords = new List<PasswordRowViewModel>();
    bool IsLoading = false;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        try
        {
            var passwordResult = await _passwordsService.GetPasswordRows(CancellationToken.None);

            passwordResult.IfSucc(p => Passwords = p);
            passwordResult.IfFail(ex => Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace));
        }
        finally
        {
            IsLoading = false;
        }
    }
}
