namespace PasswordManager.Portal.DtObjects;

public sealed record NewPassword
{
    public required Guid CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required bool IsFavorite { get; set; }
}

public sealed class PasswordRequestModel
{
    public Guid? Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public byte[] Username { get; set; } = null!;
    public byte[] Password { get; set; } = null!;
    public bool IsFavorite { get; set; } = false;
}

public sealed class PasswordResponseModel
{
    public required Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required byte[] Username { get; set; }
    public required byte[] Password { get; set; }
    public required bool IsFavorite { get; set; }
}

public sealed class PublicPasswordDataResponseModel
{
    public string? CategoryName { get; set; }
    public Guid PasswordId { get; set; }
    public string? PasswordTitle { get; set; }
    public byte[]? Username { get; set; }
    public string? Description { get; set; }
    public bool IsFavorite { get; set; }
}