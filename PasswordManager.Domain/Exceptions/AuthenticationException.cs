﻿namespace PasswordManager.Domain.Exceptions;

public sealed class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message)
    {

    }
}
