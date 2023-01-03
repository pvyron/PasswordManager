namespace PasswordManager.Application.DtObjects.Authorization;

public sealed record UserRegistrationResponseModel
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
