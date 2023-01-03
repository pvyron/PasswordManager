using System.Diagnostics.CodeAnalysis;

namespace PasswordManager.Application.DtObjects;

public sealed class ErrorResponse
{
    public ErrorResponse()
    {

    }

    [SetsRequiredMembers]
    public ErrorResponse(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
    }

    public required string Message { get; init; }

    public required Exception Exception { get; init; }
}
