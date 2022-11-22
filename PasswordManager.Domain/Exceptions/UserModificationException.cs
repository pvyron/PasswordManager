using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Domain.Exceptions;

public sealed class UserModificationException : Exception
{
    public UserModificationException(string message) : base(message)
    {

    }

    public UserModificationException(string message, UserModel userModel) : base(message)
    {
        UserModel = userModel;
    }

    public UserModel? UserModel { get; }
}
