using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
