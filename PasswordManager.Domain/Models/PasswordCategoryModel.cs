namespace PasswordManager.Domain.Models;

public sealed class PasswordCategoryModel
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}
