namespace PasswordManager.Application.DtObjects.Reporting;

public sealed class PublicPasswordDataResponseModel
{
    public string? CategoryName { get; set; }
    public Guid PasswordId { get; set; }
    public string? PasswordTitle { get; set; }
    public byte[]? Username { get; set; }
    public string? Description { get; set; }
    public bool IsFavorite { get; set; }
}
