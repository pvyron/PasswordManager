using MudBlazor;
using System.Net.Http.Json;
using PasswordManager.Portal.ViewModels.Dashboard;
using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.ViewPasswords;
using System.Runtime.CompilerServices;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class ViewPasswords
{
    [Inject] PasswordsService PasswordsService { get; set; } = default!;
    MudDataGrid<PasswordRowViewModel>? DataGrid { get; set; }

    List<PasswordRowViewModel> _passwords = new();
    bool _isLoading = false;
    string? SearchText { get; set; }
    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        try
        {
            var passwordResult = await PasswordsService.GetPasswordRows(CancellationToken.None);

            passwordResult.IfSucc(p => _passwords = p);
            passwordResult.IfFail(ex => Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace));
        }
        finally
        {
            _isLoading = false;
        }
    }

    bool SearchFilter(PasswordRowViewModel passwordRow)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return true;

        if (passwordRow.PasswordTitle?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) 
            return true;

        if (passwordRow.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        if (passwordRow.CategoryName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        return false;
    }

    void OnSearch()
    {
        DataGrid?.ExpandAllGroups();
    }
}
