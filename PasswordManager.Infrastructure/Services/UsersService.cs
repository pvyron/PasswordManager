using LanguageExt.Pretty;
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
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

internal sealed class UsersService : IUsersService
{
    private const string USER_TABLE_NAME = "users";

    private readonly MDbClient _dbClient;

    public UsersService(MDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public async IAsyncEnumerable<UserModel> GetAllUsers([EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var userDbModel in await _dbClient.GetAllRecords<UserDbModel>(USER_TABLE_NAME, cancellationToken))
        {
            yield return new UserModel
            {
                Id = userDbModel.Id,
                Email = userDbModel.Email,
                FirstName = userDbModel.FirstName,
                LastName = userDbModel.LastName,
            };
        }
    }

    public async Task<UserModel> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel>(USER_TABLE_NAME, id, cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User {id} was not found");

        return new UserModel
        {
            Id = userDbModel.Id,
            Email = userDbModel.Email,
            FirstName = userDbModel.FirstName,
            LastName = userDbModel.LastName
        };
    }

    public async Task<UserModel> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecord<UserDbModel>(USER_TABLE_NAME, (nameof(UserDbModel.Email), email), cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User with email {email} was not found");

        return new UserModel
        {
            Id = userDbModel.Id,
            Email = userDbModel.Email,
            FirstName = userDbModel.FirstName,
            LastName = userDbModel.LastName
        };
    }

    public async Task<UserModel> CreateUser(UserModel user, string password, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecord<UserDbModel?>(USER_TABLE_NAME, (nameof(UserDbModel.Email), user.Email), cancellationToken);

        if (userDbModel is not null)
        {
            throw new UserAccessException($"Email {user.Email} is already registered");
        }

        var newUserDbModel = new UserDbModel
        {
            Email= user.Email,
            FirstName = user.FirstName,
            LastName= user.LastName,
            Password = password
        };

        var createdUser = await _dbClient.InsertRecord(USER_TABLE_NAME, newUserDbModel, cancellationToken);

        return new UserModel
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName
        };
    }

    public async Task<UserModel> UpdateUser(UserModel user, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel?>(USER_TABLE_NAME, user.Id, cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User with email {user.Email} was not found");

        userDbModel.Email = user.Email;
        userDbModel.FirstName = user.FirstName;
        userDbModel.LastName = user.LastName;

        var updatedUser = await _dbClient.UpdateRecord(USER_TABLE_NAME, user.Id, userDbModel, cancellationToken);

        return new UserModel
        {
            Id = updatedUser.Id,
            Email = updatedUser.Email,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName
        };
    }

    public async Task<UserModel> UpdateUserPassword(Guid id, string newPassword, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel?>(USER_TABLE_NAME, id, cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User {id} was not found");

        userDbModel.Password = newPassword;

        var updatedUser = await _dbClient.UpdateRecord(USER_TABLE_NAME, id, userDbModel, cancellationToken);

        return new UserModel
        {
            Id = updatedUser.Id,
            Email = updatedUser.Email,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName
        };
    }

    public async Task DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel?>(USER_TABLE_NAME, id, cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User {id} was not found");

        userDbModel.IsActive = false;

        await _dbClient.UpdateRecord(USER_TABLE_NAME, id, userDbModel, cancellationToken);
    }
}
