using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models.RequestModels
{
    public class UserRequestModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
    }
}
