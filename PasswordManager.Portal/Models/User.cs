namespace PasswordManager.Portal.Models;

public sealed class User
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required bool RemainLoggedIn { get; set; }
    public required string AccessToken { get; set; }
    public required byte[] DecryptionToken { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
