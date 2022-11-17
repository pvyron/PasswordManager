using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models.ResponseModels
{
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
