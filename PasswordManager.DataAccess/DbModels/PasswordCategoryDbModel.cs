using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DataAccess.DbModels;

public class PasswordCategoryDbModel
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey(nameof(User))]
    public Guid? UserId { get; set; }
    public virtual UserDbModel? User { get; set; }
    public required string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EditedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<PasswordDbModel>? Passwords { get; set; }
}
