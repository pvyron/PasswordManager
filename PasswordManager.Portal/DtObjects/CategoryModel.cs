namespace PasswordManager.Portal.DtObjects;

public sealed class CategoryResponseModel
{
    public Guid? Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}