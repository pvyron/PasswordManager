namespace PasswordManager.Domain.Models;

public sealed class UserModel
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();
}
