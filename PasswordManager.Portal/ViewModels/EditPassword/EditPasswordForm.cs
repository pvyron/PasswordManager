using PasswordManager.Portal.ViewModels.AddPassword;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Portal.ViewModels.EditPassword;

public sealed class EditPasswordForm
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "A title is mandatory")]
    [DataType(DataType.Text)]
    public string Title { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is mandatory")]
    [DataType(DataType.Text)]
    public string Username { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is mandatory")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    public bool? Favorite { get; set; } = false;

    [Required(ErrorMessage = "Category is mandatory")]
    public AvailableCategory Category { get; set; } = null!;



    public bool ShowPassword { get; set; } = false;

    public bool IsValid { get; set; }

    public void LoadPassword(PasswordViewModel password, List<AvailableCategory> availableCategories)
    {
        Title = password.Title!;
        Username = password.Username!;
        Password = password.Password!;
        Description = password.Description;
        Category = availableCategories.First(c => c.Id == password.CategoryId);
    }
}
