namespace PasswordManager.Domain.Models;

public sealed class PasswordModel
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid? CategoryId { get; set; }
    public required Guid? ImageId { get; set; }
    public required string Title { get; set; }
    public required string? Description { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }
    public required bool? IsFavorite { get; set; }
    public required PasswordLogoModel? Logo { get; set; }
}
