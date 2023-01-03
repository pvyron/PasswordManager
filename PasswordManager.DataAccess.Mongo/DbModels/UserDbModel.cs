using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.DataAccess.DbModels;

public sealed class UserDbModel
{
    [BsonId]
    public Guid Id { get; set; }
    [BsonRequired]
    public required string Email { get; set; } = null!;
    [BsonRequired]
    public required string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public List<PasswordCategoryDbModel> Categories { get; set; } = new();
}
