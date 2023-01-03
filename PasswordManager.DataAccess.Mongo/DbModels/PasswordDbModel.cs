using MongoDB.Bson.Serialization.Attributes;

namespace PasswordManager.DataAccess.DbModels;

public sealed class PasswordDbModel
{
    [BsonRequired]
    public required Guid Id { get; set; }
    [BsonRequired]
    public required string Title { get; set; } = null!;
    public string? Description { get; set; }
    [BsonRequired]
    public required string Username { get; set; } = null!;
    [BsonRequired]
    public required string Password { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
