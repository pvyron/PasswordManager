namespace PasswordManager.Domain.Models;

public sealed class PasswordModel
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
