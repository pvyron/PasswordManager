using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Application.DtObjects.Passwords;

public sealed record PasswordRequestModel
{
    public Guid? Id { get; set; }
    public Guid? CategoryId { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password needs a name")]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a username")]
    public string Username { get; set; } = null!;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a password")]
    public string Password { get; set; } = null!;
}
