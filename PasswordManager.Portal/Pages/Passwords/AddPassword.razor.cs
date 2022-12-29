using Microsoft.AspNetCore.Components;
using MudBlazor;
using PasswordManager.Portal.Pages.Categories;
using PasswordManager.Portal.Services;
using PasswordManager.Portal.ViewModels.AddPassword;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Text.RegularExpressions;

namespace PasswordManager.Portal.Pages.Passwords;

public partial class AddPassword
{
    [Inject] CategoriesService _categoriesService { get; set; } = default!;
    AddPasswordForm _addPasswordForm { get; set; } = new();
    List<AvailableCategory> Categories { get; set; } = new();







    bool success = false;
    string[] errors = { };
    MudTextField<string> pwField1;
    MudForm form;
    MudForm form2;

    protected override async Task OnInitializedAsync()
    {
        var result = await _categoriesService.GetAllCategories(CancellationToken.None);

        result.IfSucc(PasswordFetchingSuccess);

        await base.OnInitializedAsync();
    }

    private void PasswordFetchingSuccess(List<AvailableCategory> categories)
    {
        Categories = categories;
    }

    private IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }
        if (pw.Length < 8)
            yield return "Password must be at least of length 8";
        if (!Regex.IsMatch(pw, @"[A-Z]"))
            yield return "Password must contain at least one capital letter";
        if (!Regex.IsMatch(pw, @"[a-z]"))
            yield return "Password must contain at least one lowercase letter";
        if (!Regex.IsMatch(pw, @"[0-9]"))
            yield return "Password must contain at least one digit";
    }

    private string PasswordMatch(string arg)
    {
        if (pwField1.Value != arg)
            return "Passwords don't match";
        return null;
    }
}
