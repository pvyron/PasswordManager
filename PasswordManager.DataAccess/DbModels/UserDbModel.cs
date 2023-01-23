using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DataAccess.DbModels;

[Index(nameof(Email), IsUnique = true)]
public class UserDbModel
{
    [Key]
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EditedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<PasswordDbModel>? Passwords { get; set; }
    public ICollection<PasswordCategoryDbModel>? Categories { get; set; }
}
