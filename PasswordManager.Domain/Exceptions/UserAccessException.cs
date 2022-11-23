using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
