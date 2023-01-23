using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DataAccess.DbModels;

public class PasswordDbModel
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey(nameof(Category))]
    public Guid? CategoryId { get; set; }
    [ForeignKey(nameof(User))]
    public Guid? UserId { get; set; }
    [ForeignKey(nameof(Image))]
    public Guid? ImageId { get; set; }
    public virtual UserDbModel? User { get; set; }
    public virtual PasswordCategoryDbModel? Category { get; set; }
    public virtual PasswordLogoDbModel? Image { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }
    public required bool IsFavorite { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EditedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;
}
