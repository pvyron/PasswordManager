using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.DbModels;

public class PasswordLogoDbModel
{
    [Key]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string BulkStorageImageName { get; set; }
    public required string ImageUrl { get; set; }
    public required string BulkStorageThumbnailName { get; set; }
    public required string ThumbnailUrl { get; set; }
    public ICollection<PasswordDbModel>? Passwords { get; set; }
}
