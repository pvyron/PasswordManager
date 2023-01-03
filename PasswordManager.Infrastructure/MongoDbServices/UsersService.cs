using Bogus;
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
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.MongoDbServices;

internal sealed class UsersService : UsersTableAccessService, IUsersService
{
    private readonly MDbClient _dbClient;

    public UsersService(MDbClient dbClient) : base(dbClient)
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
        var userDbModel = await GetUserDbModel(id, cancellationToken);

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
        var userDbModel = await _dbClient.GetRecord<UserDbModel>(USER_TABLE_NAME, (nameof(UserDbModel.Email), user.Email), cancellationToken);

        if (userDbModel is not null)
        {
            throw new UserAccessException($"Email {user.Email} is already registered");
        }

        var newUserDbModel = new UserDbModel
        {
            Email= user.Email,
            FirstName = user.FirstName,
            LastName= user.LastName,
            Password = password,
            Categories = new List<PasswordCategoryDbModel>
            {
                new PasswordCategoryDbModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Default Category",
                    Passwords = new List<PasswordDbModel>
                    {
                        new PasswordDbModel
                        {
                            Id= Guid.NewGuid(),
                            Title = "Default Password",
                            Username = "Username",
                            Password = "Password"
                        }
                    }
                }
            }
        };

        var createdUser = await _dbClient.InsertRecord(USER_TABLE_NAME, newUserDbModel, cancellationToken);

        return new UserModel
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
        };
    }

    public async Task<UserModel> UpdateUser(UserModel user, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel>(USER_TABLE_NAME, user.Id, cancellationToken);

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
            LastName = updatedUser.LastName,
        };
    }

    public async Task<UserModel> UpdateUserPassword(Guid id, string newPassword, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel>(USER_TABLE_NAME, id, cancellationToken);

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

    public async Task PopulateDb(CancellationToken cancellationToken)
    {
        var faker = new Faker();

        List<UserDbModel> result = faker.Make(2, _ =>
        {
            return new UserDbModel 
            { 
                Email = faker.Internet.Email(provider: "pvyron.com"),
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                IsActive= true,
                Password = "1234",
                Categories = faker.Make(2, _ =>
                {
                    return new PasswordCategoryDbModel
                    {
                        IsActive = true,
                        Description = faker.Lorem.Sentence(3),
                        Id = Guid.NewGuid(),
                        Title = faker.Company.CompanyName(),
                        Passwords = faker.Make(5, _ =>
                        {
                            return new PasswordDbModel
                            {
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                Title = faker.Company.CompanyName(),
                                Description = faker.Lorem.Sentences(3),
                                Password = faker.Internet.Password(5),
                                Username = faker.Internet.UserName()
                            };
                        }).ToList()
                    };
                }).ToList()
            };
        }).ToList();

        foreach (var res in result)
        {
            await _dbClient.InsertRecord(USER_TABLE_NAME, res, cancellationToken);
        }

        
    }
}
