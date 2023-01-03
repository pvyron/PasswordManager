using Bogus;
using LanguageExt.Pretty;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

internal sealed class UsersService : IUsersService
{
    private readonly AzureMainDatabaseContext _context;

    public UsersService(AzureMainDatabaseContext context)
    {
        _context = context;
    }

    public async Task<UserModel> CreateUser(UserModel user, string password, CancellationToken cancellationToken)
    {
        var addedUserResponse = await _context.Users.AddAsync(new UserDbModel 
        { 
            Email= user.Email,
            Password = password,
            FirstName= user.FirstName,
            LastName= user.LastName
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new UserModel
        {
            Id = addedUserResponse.Entity.Id,
            Email = addedUserResponse.Entity.Email,
            FirstName = addedUserResponse.Entity.FirstName,
            LastName = addedUserResponse.Entity.LastName
        };
    }

    public Task DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<UserModel> GetAllUsers(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
        {
            throw new UserAccessException($"User with email {email} was not found");
        }

        return new UserModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<UserModel> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(id, cancellationToken);

        if (user is null)
        {
            throw new UserAccessException($"User with id {id} was not found");
        }

        return new UserModel
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public Task PopulateDb(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> UpdateUser(UserModel user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> UpdateUserPassword(Guid id, string newPassword, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
