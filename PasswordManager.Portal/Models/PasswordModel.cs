namespace PasswordManager.Portal.Models;

public sealed class PasswordModel
{
    public required Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Title { get; set; }
    public required string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required bool? IsFavorite { get; set; }
    public required LogoModel? Logo { get; set; }
}
