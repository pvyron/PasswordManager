using PasswordManager.Domain.Models;

namespace PasswordManager.Domain.Exceptions;

public sealed class PasswordCategoryAccessException : AccessException<PasswordCategoryModel>
{
    public PasswordCategoryAccessException(string message) : base(message)
    {

    }

    public PasswordCategoryAccessException(string message, PasswordCategoryModel passwordCategoryModel) : base(message)
    {
        Model = passwordCategoryModel;
    }

    public override PasswordCategoryModel? Model { get; init; }
}
