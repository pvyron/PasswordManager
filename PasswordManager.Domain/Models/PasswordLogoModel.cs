namespace PasswordManager.Domain.Models;

public sealed class PasswordLogoModel
{
    public required Guid LogoId { get; set; }
    public required string Title { get; set; }
    public required string FileUrl { get; set; }
    public required string FileExtension { get; set; }

    public string FileLocation => $"{FileUrl}.{FileExtension}";
}
