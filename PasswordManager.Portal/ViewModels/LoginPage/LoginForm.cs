using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Portal.ViewModels.LoginPage;

public class LoginForm
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool RememberMe { get; set; } = true;
    public string? ErrorMessage { get; set; }

    public bool ShowPassword { get; set; } = false;
    public bool IsValid { get; set; }
}
