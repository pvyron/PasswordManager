using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
