namespace PasswordManager.Portal.Models;

public sealed class LogoModel
{
    public required Guid ImageId { get; set; }
    public required string Title { get; set; }
    public required string FileUrl { get; set; }
    public required string ThumbnailUrl { get; set; }
}
