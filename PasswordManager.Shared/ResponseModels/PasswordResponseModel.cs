namespace PasswordManager.Shared.ResponseModels;

public sealed record PasswordResponseModel
{
    public required Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ImageId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }
    public bool? IsFavorite { get; set; }
    public string? ImageTitle { get; set; }
    public string? PublicUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
}
