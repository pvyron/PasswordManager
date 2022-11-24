using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Portal.ViewModels.RegisterPage;

public sealed class RegisterForm
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is mandatory")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Provide a password")]
    [DataType(DataType.Password)]
    public string OriginalPassword { get; set; } = null!;

    [Compare(nameof(OriginalPassword), ErrorMessage = "Passwords don't match")]
    [DataType(DataType.Password)]
    public string ConfirmationPassword { get; set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool ShowOriginalPassword { get; set; } = false;
    public bool ShowConfirmationPassword { get; set; } = false;
}
