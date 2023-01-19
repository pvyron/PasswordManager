using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;

namespace PasswordManager.Infrastructure.Services;

internal sealed class UsersService : IUsersService
{
    private readonly ISqlDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public UsersService(ISqlDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    public async Task<UserModel> CreateUser(UserModel user, string password, CancellationToken cancellationToken)
    {
        (var hashedPassword, var privateSalt) = _authorizationService.GenerateHashedPassword(password);

        var emailExists = _context.Users.Exists(u => u.Email == user.Email);

        if (emailExists)
        {
            throw new UserAccessException("Email may be already in use", user);
        }

        var addedUserResponse = await _context.Users.AddAsync(new UserDbModel
        {
            Email = user.Email,
            Password = hashedPassword,
            PasswordSalt = privateSalt,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Categories = new List<PasswordCategoryDbModel>
            {
                new PasswordCategoryDbModel
                {
                    Title = "Default Category",
                    Description = "Feel free to delete it or modify it as you please, you alway need to have at least one category for your passwords to fall under",
                }
            }
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

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

    public async Task ResetAllUsersPassword(string newPassword, CancellationToken cancellationToken)
    {
        var allUsers = _context.Users;

        foreach (var user in allUsers)
        {
            (var hashedPassword, var privateSalt) = _authorizationService.GenerateHashedPassword(newPassword);

            user.Password = hashedPassword;
            user.PasswordSalt = privateSalt;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
