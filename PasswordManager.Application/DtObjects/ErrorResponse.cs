using System.Diagnostics.CodeAnalysis;

namespace PasswordManager.Application.DtObjects;

public sealed class ErrorResponse
{
    public ErrorResponse()
    {

    }

    [SetsRequiredMembers]
    public ErrorResponse(string message)
    {
        Message = message;
    }

    public required string Message { get; init; }
}
