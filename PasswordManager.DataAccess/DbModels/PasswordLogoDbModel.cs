using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DataAccess.DbModels;

public class PasswordLogoDbModel
{
    [Key]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string BulkStorageImageName { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<PasswordDbModel>? Passwords { get; set; }
}
