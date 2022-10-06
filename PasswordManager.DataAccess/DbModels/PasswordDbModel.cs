using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DataAccess.DbModels
{
    [Table("Passwords")]
    public sealed class PasswordDbModel : IDbModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Title { get; set; } = null!;

        [MaxLength(255)]
        public string? Details { get; set; }

        public string? Comments { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }

        [Required]
        public UserDbModel Owner { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string PasswordText { get; set; } = null!;

        [Required]
        public bool Active { get; set; } = true;
    }
}
