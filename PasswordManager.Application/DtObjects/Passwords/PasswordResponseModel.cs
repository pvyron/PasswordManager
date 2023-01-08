namespace PasswordManager.Application.DtObjects.Passwords;

public sealed record PasswordResponseModel
{
    public required Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }
    public required bool IsFavorite { get; set; }
}
