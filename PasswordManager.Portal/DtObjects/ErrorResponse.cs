namespace PasswordManager.Portal.DtObjects;

public sealed class ErrorResponseModel
{
    public string? Message { get; set; }

    public Exception? Exception { get; set; }
}
