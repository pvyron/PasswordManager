namespace PasswordManager.Portal.ViewModels.AddPassword;

public sealed class AddPasswordForm
{
    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Description { get; set; }

    public AvailableCategory Category { get; set; } = null!;



    public bool ShowPassword { get; set; } = false;

    public bool IsValid { get; set; }
}


