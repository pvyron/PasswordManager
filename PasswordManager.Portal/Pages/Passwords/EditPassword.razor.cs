﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Components;
using PasswordManager.Portal.Constants;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.AddPassword;
using PasswordManager.Portal.ViewModels.Dashboard;
using PasswordManager.Portal.ViewModels.EditPassword;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class EditPassword
{
    [Inject] PasswordsService PasswordsService { get; set; } = default!;
    [Inject] CategoriesService CategoriesService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;

    [Parameter] public string? PassId { get; set; }

    MudForm? UiForm { get; set; }

    EditPasswordForm EditPasswordForm { get; set; } = new();
    PasswordViewModel Password { get; set; } = new();
    List<AvailableCategory> AvailableCategories { get; set; } = new();
    bool SavingPasswordInProgress { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        var passwordFetching = PasswordFetching();
        var categoriesFetching = CategoriesFetching();

        await Task.WhenAll(passwordFetching, categoriesFetching);

        EditPasswordForm.LoadPassword(Password, AvailableCategories);

        await base.OnInitializedAsync();
    }

    async Task PasswordFetching()
    {
        var passwordResponse = await PasswordsService.GetPasswordById(PassId, CancellationToken.None);

        passwordResponse.IfSucc(SuccessfullPasswordFetching);
        passwordResponse.IfFail(async ex => await FailedFetching(ex));
    }

    async Task CategoriesFetching()
    {
        var categoriesResponse = await CategoriesService.GetAllCategories(CancellationToken.None);

        categoriesResponse.IfSucc(SuccessfullCategoriesFetching);
        categoriesResponse.IfFail(async ex => await FailedFetching(ex));
    }

    void SuccessfullPasswordFetching(PasswordViewModel password)
    {
        Password = password;
    }

    void SuccessfullCategoriesFetching(List<AvailableCategory> categories)
    {
        AvailableCategories = categories;
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

        NavigationManager.NavigateTo(ApplicationRoutes.Index);
    }

    void PickedCategoryChanged(AvailableCategory availableCategory)
    {
        EditPasswordForm.Category = availableCategory;
    }

    private async Task SavePassword()
    {
        if (UiForm is null)
            return;

        await UiForm.Validate();

        if (!EditPasswordForm.IsValid)
        {
            return;
        }

        try
        {
            SavingPasswordInProgress = true;

            var result = await PasswordsService.UpdatePassword(Guid.Parse(PassId!), new NewPassword
            {
                CategoryId = EditPasswordForm.Category.Id!.Value,
                Title = EditPasswordForm.Title,
                Username = EditPasswordForm.Username,
                Description = EditPasswordForm.Description,
                Password = EditPasswordForm.Password,
                IsFavorite = EditPasswordForm.Favorite.GetValueOrDefault(false)
            }, CancellationToken.None);

            result.IfSucc(async p => await SuccessfullUpdatePassword(p));
            result.IfFail(async ex => await FailedUpdatePassword(ex));
        }
        finally
        {
            SavingPasswordInProgress = false;
        }
    }

    async Task SuccessfullUpdatePassword(PasswordViewModel passwordViewModel)
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

        EditPasswordForm.LoadPassword(Password = passwordViewModel, AvailableCategories);
    }

    async Task FailedUpdatePassword(Exception ex)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            Position = DialogPosition.Center,
            MaxWidth = MaxWidth.Medium
        };

        var parameters = new DialogParameters
        {
            { "Message", $"Password update failed with error {ex.Message}" }
        };

        var dialog = DialogService.Show<NotifyDialog>("Failed", parameters, options);

        await dialog.Result;
    }
}