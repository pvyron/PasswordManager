namespace PasswordManager.DataAccess.DbModels;

public class UserDbModel
{
    public Guid Id { get; set; }
    public required string Email { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<PasswordDbModel>? Passwords { get; set; }
    public ICollection<PasswordCategoryDbModel>? Categories { get; set; }
}
