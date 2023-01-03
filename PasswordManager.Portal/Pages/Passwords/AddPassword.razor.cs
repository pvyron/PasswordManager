using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.AddPassword;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class AddPassword
{
    [Inject] NavigationManager _navManager { get; set; } = default!;
    [Inject] IDialogService _dialogService { get; set; } = default!;
    [Inject] CategoriesService _categoriesService { get; set; } = default!;
    [Inject] PasswordsService _passwordsService { get; set; } = default!;
    AddPasswordForm _addPasswordForm { get; set; } = new();
    List<AvailableCategory> AvailableCategories { get; set; } = new();
    bool _addingPasswordInProgress { get; set; } = false;


    protected override async Task OnInitializedAsync()
    {
        var result = await _categoriesService.GetAllCategories(CancellationToken.None);

        result.IfSucc(CategoriesFetchingSuccess);

        await base.OnInitializedAsync();
    }

    private void CategoriesFetchingSuccess(List<AvailableCategory> categories)
    {
        AvailableCategories = categories;
    }

    private void PickedCategoryChanged(AvailableCategory availableCategory)
    {
        _addPasswordForm.Category = availableCategory;
    }

    private async Task AddNewPassword()
    {
        if (!_addPasswordForm.IsValid)
        {
            return;
        }

        try
        {
            _addingPasswordInProgress = true;

            var result = await _passwordsService.AddNewPassword(new NewPassword
            {
                CategoryId = _addPasswordForm.Category.Id!.Value,
                Title = _addPasswordForm.Title,
                Username = _addPasswordForm.Username,
                Description = _addPasswordForm.Description,
                Password = _addPasswordForm.Password
            }, CancellationToken.None);

            result.IfSucc(async _ => await SuccessfullAddPassword());
            result.IfFail(async ex => await FailedAddPassword(ex));
        }
        finally
        {
            _addingPasswordInProgress = false;
        }
    }

    async Task SuccessfullAddPassword()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters
        {
            { "Message", "Password was saved successfully" }
        };

        var dialog = _dialogService.Show<NotifyDialog>("Success", parameters, options);

        await dialog.Result;

        _navManager.NavigateTo(ApplicationRoutes.Dashboard);
    }

    async Task FailedAddPassword(Exception ex)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters
        {
            { "Message", $"Password addition failed with error {ex.Message}" }
        };

        var dialog = _dialogService.Show<NotifyDialog>("Failed", parameters, options);

        await dialog.Result;
    }
}
