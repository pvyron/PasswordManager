using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Portal.DtObjects;

public sealed record NewPassword
{
    public required Guid CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public sealed class PasswordRequestModel
{
    public Guid? Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public sealed class PasswordResponseModel
{
    public required Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
