using MudBlazor;
using System.Net.Http.Json;
using PasswordManager.Portal.ViewModels.Dashboard;
using Microsoft.AspNetCore.Components;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.ViewPasswords;
using System.Runtime.CompilerServices;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Constants;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class ViewPasswords
{
    [Inject] PasswordsService PasswordsService { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

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

    async Task OnViewPasswordClicked(Guid passwordId)
    {
        var passwordResponse = await PasswordsService.GetPasswordById(passwordId.ToString(), CancellationToken.None);

        passwordResponse.IfSucc(async succ => await SuccessfullPasswordFetching(succ));
        passwordResponse.IfFail(async ex => await FailedFetching(ex));
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

    static void OnSearch()
    {
        
    }

    async Task SuccessfullPasswordFetching(PasswordViewModel password)
    {
        var options = new DialogOptions
        {
            CloseButton = false,
            CloseOnEscapeKey = true,
            DisableBackdropClick = false,
            FullWidth = true,
            FullScreen = false,
            NoHeader = false,
            Position = DialogPosition.Center
        };

        var parameters = new DialogParameters
        {
            {"Password", password }
        };

        var dialog = DialogService.Show<PasswordCredentialsDialog>(password.Title, parameters, options);

        await dialog.Result;
    }

    async Task FailedFetching(Exception ex)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters
        {
            { "Message", $"{ex.Message}" }
        };

        var dialog = DialogService.Show<NotifyDialog>("Failed", parameters, options);

        await dialog.Result;

        NavigationManager.NavigateTo(ApplicationRoutes.ViewPasswords);
    }
}
