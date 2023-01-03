using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Exceptions;

public sealed class UserAccessException : AccessException<UserModel>
{
    public UserAccessException(string message) : base(message)
    {

    }

    public UserAccessException(string message, UserModel userModel) : base(message)
    {
        Model = userModel;
    }

    public override UserModel? Model { get; init; }
}
