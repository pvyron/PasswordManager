namespace PasswordManager.Application.DtObjects.Authorization;

public sealed record UserLoginResponseModel
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string AccessToken { get; set; }
}
