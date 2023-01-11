namespace PasswordManager.Portal.ViewModels.ViewPasswords;

public sealed class PasswordRowViewModel
{
    public Guid PasswordId { get; set; }
    public string? CategoryName { get; set; }
    public string? PasswordTitle { get; set; }
    public string? Username { get; set; }
    public string? Description { get; set; }
    public bool IsFavorite { get; set; }
}
