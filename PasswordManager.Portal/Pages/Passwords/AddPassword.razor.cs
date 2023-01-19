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
    [Inject] NavigationManager NavManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] CategoriesService CategoriesService { get; set; } = default!;
    [Inject] PasswordsService PasswordsService { get; set; } = default!;

    MudForm? UiForm { get; set; }

    AddPasswordForm AddPasswordForm { get; set; } = new();
    List<AvailableCategory> AvailableCategories { get; set; } = new();
    bool FormComponentsDisabled => PasswordFetchingInProgress && AddingPasswordInProgress;
    bool AddingPasswordInProgress { get; set; } = false;
    bool PasswordFetchingInProgress { get; set; } = false;


    protected override async Task OnInitializedAsync()
    {
        try
        {
            PasswordFetchingInProgress = true;
            var result = await CategoriesService.GetAllCategories(CancellationToken.None);

            result.IfSucc(CategoriesFetchingSuccess);
        }
        finally
        {
            PasswordFetchingInProgress = false;
        }

        await base.OnInitializedAsync();
    }

    private void CategoriesFetchingSuccess(List<AvailableCategory> categories)
    {
        AvailableCategories = categories;
    }

    private void PickedCategoryChanged(AvailableCategory availableCategory)
    {
        AddPasswordForm.Category = availableCategory;
    }

    private async Task AddNewPassword()
    {
        if (UiForm is null)
            return;

        await UiForm.Validate();

        if (!AddPasswordForm.IsValid)
        {
            return;
        }

        try
        {
            AddingPasswordInProgress = true;

            var result = await PasswordsService.AddNewPassword(new NewPassword
            {
                CategoryId = AddPasswordForm.Category.Id!.Value,
                Title = AddPasswordForm.Title,
                Username = AddPasswordForm.Username,
                Description = AddPasswordForm.Description,
                Password = AddPasswordForm.Password,
                IsFavorite = AddPasswordForm.Favorite.GetValueOrDefault(false)
            }, CancellationToken.None);

            result.IfSucc(async _ => await SuccessfullAddPassword());
            result.IfFail(async ex => await FailedAddPassword(ex));
        }
        finally
        {
            AddingPasswordInProgress = false;
        }
    }
    private async Task GenerateRandomPasswordButtonClicked()
    {
        var dialog = DialogService.Show<RandomPasswordDialog>();

        var result = await dialog.Result;

        if (result.Canceled)
            return;

        if (result.Data is string generatedPassword)
            AddPasswordForm.Password = generatedPassword;

        StateHasChanged();
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

        var dialog = DialogService.Show<NotifyDialog>("Success", parameters, options);

        await dialog.Result;

        NavManager.NavigateTo(ApplicationRoutes.Dashboard);
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

        var dialog = DialogService.Show<NotifyDialog>("Failed", parameters, options);

        await dialog.Result;
    }
}
