using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.DataAccess.DbModels;

public sealed class PasswordCategoryDbModel
{
    [BsonRequired]
    public required Guid Id { get; set; }
    [BsonRequired]
    public required string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public List<PasswordDbModel> Passwords { get; set; } = new();
}
