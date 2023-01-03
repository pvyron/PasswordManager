namespace PasswordManager.Application.DtObjects.Categories;

public sealed class CategoryResponseModel
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}
