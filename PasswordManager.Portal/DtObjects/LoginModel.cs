namespace PasswordManager.Portal.DtObjects;

public sealed record LoginModel(string Email, string Password);

public sealed class LoginReponseModel
{
    public string Email { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string? FirstName { get; set;  }
    public string? LastName { get; set; }
}