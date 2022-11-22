using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.DtObjects.Authorization;

public sealed record UserRegistrationRequestModel
{
    [Required]
    public string Email { get; set; } = null!;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Need a valid password")]
    public string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
