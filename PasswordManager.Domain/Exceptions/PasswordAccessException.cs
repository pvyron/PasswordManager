using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Exceptions;

public sealed class PasswordAccessException : AccessException<PasswordModel>
{
    public PasswordAccessException(string message) : base(message)
    {

    }

    public PasswordAccessException(string message, PasswordModel passwordModel) : base(message)
    {
        Model = passwordModel;
    }

    public override PasswordModel? Model { get; init; }
}
