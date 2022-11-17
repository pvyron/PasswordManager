using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DataAccess.DbModels
{
    [Table("Users")]
    public sealed class UserDbModel : IDbModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string LastName { get; set; } = null!;
    }
}
