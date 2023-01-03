namespace PasswordManager.Portal.DtObjects;

public sealed record RegistrationModel(string Email, string Password, string? FirstName, string? LastName);

public sealed class RegistrationRequestModel
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}