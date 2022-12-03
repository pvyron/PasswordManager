using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.DbModels;

public sealed class UserDbModel
{
    [BsonId]
    public Guid Id { get; set; }
    [BsonRequired]
    public required string Email { get; set; }
    [BsonRequired]
    public required string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public List<PasswordCategoryDbModel>? Categories { get; set; }
}
