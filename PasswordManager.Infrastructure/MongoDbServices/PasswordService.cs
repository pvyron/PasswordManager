using Bogus;
using LanguageExt.Pipes;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.MongoDbServices;

internal sealed class PasswordService : UsersTableAccessService, IPasswordService
{
    public PasswordService(MDbClient dbClient) : base(dbClient)
    {
    }

    public async IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(userId, cancellationToken);

        foreach (var passwordCategoryDbModel in userDbModel.Categories)
        {
            if (passwordCategoryDbModel.Passwords.Count == 0)
                continue;

            foreach (var passwordDbModel in passwordCategoryDbModel.Passwords)
            {
                yield return new PasswordModel
                {
                    Description = passwordDbModel.Description,
                    Id = passwordDbModel.Id,
                    Title = passwordDbModel.Title,
                    UserId = userId,
                    CategoryId = passwordCategoryDbModel.Id,
                    Password = passwordDbModel.Password,
                    Username = passwordDbModel.Username,
                };
            }
        }
    }

    public async Task<PasswordModel> GetPasswordById(Guid userId, Guid passwordId, CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(userId, cancellationToken);

        foreach (var passwordCategoryDbModel in userDbModel.Categories)
        {
            var passwordDbModel = passwordCategoryDbModel.Passwords.Find(p => p.Id == passwordId);

            if (passwordDbModel is null)
                continue;

            return new PasswordModel
            {
                CategoryId = passwordCategoryDbModel.Id,
                Description = passwordDbModel.Description,
                Id = passwordDbModel.Id,
                Password = passwordDbModel.Password,
                Title = passwordDbModel.Title,
                UserId = userDbModel.Id,
                Username = passwordDbModel.Username,
            };
        }

        throw new PasswordAccessException($"Password {passwordId} was not found");
    }

    public async Task<PasswordModel> GetPasswordById(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(userId, cancellationToken);

        var passwordCategoryDbModel = userDbModel.Categories.Find(c => c.Id == categoryId);

        if (passwordCategoryDbModel is null)
            throw new PasswordAccessException($"Category {categoryId} was not found");

        var passwordDbModel = passwordCategoryDbModel.Passwords.Find(p => p.Id == passwordId);

        if (passwordDbModel is null)
            throw new PasswordAccessException($"Password {passwordId} was not found");

        return new PasswordModel
        {
            CategoryId = passwordCategoryDbModel.Id,
            Description = passwordDbModel.Description,
            Id = passwordDbModel.Id,
            Password = passwordDbModel.Password,
            Title = passwordDbModel.Title,
            UserId = userDbModel.Id,
            Username = passwordDbModel.Username,
        };
    }

    public async Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(password.UserId, cancellationToken);

        PasswordCategoryDbModel? categoryDbModel;

        if (password.CategoryId is not null)
        {
            categoryDbModel = userDbModel.Categories.Find(c => c.Id == password.CategoryId);

            if (categoryDbModel is null)
                throw new PasswordCategoryAccessException($"Category {password.CategoryId} was not found");
        }
        else
        {
            categoryDbModel = userDbModel.Categories.Find(c => c.Title == "Default");
        }

        categoryDbModel ??= new()
        {
            Id = Guid.NewGuid(),
            Title = "Default",
        };

        var newPasswordDbModel = new PasswordDbModel
        {
            Id = password.Id,
            Description = password.Description,
            Password = password.Password,
            Title = password.Title,
            Username = password.Username,
        };

        categoryDbModel.Passwords.Add(newPasswordDbModel);

        if (!userDbModel.Categories!.Contains(categoryDbModel))
            userDbModel.Categories.Add(categoryDbModel);

        await UpdateUserDbModel(userDbModel, cancellationToken);

        return new PasswordModel
        {
            CategoryId = categoryDbModel.Id,
            Description = newPasswordDbModel.Description,
            Id = newPasswordDbModel.Id,
            Password = newPasswordDbModel.Password,
            Title = newPasswordDbModel.Title,
            UserId = userDbModel.Id,
            Username = newPasswordDbModel.Username,
        };
    }

    public async Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(password.UserId, cancellationToken);

        if (password.CategoryId is null)
            throw new PasswordAccessException("Category id is mandatory on update", password);

        var passwordCategoryDbModel = userDbModel.Categories.Find(c => c.Id == (Guid)password.CategoryId);

        if (passwordCategoryDbModel is null)
            throw new PasswordCategoryAccessException($"Category {password.CategoryId} was not found");

        var existingPassword = passwordCategoryDbModel.Passwords.Find(p => p.Id == password.Id);

        if (existingPassword is null)
            throw new PasswordAccessException($"Password {password.Id} was not found", password);

        existingPassword.Description = password.Description;
        existingPassword.Username = password.Username;
        existingPassword.Password = password.Password;
        existingPassword.Username = password.Username;
        existingPassword.Title = password.Title;
        existingPassword.Password = password.Password;

        await UpdateUserDbModel(userDbModel, cancellationToken);

        return new PasswordModel
        {
            CategoryId = passwordCategoryDbModel.Id,
            Description = existingPassword.Description,
            Id = existingPassword.Id,
            Password = existingPassword.Password,
            Title = existingPassword.Title,
            UserId = userDbModel.Id,
            Username = existingPassword.Username,
        };
    }

    public async Task DeletePassword(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken)
    {
        var userDbModel = await GetUserDbModel(userId, cancellationToken);

        var passwordCategoryDbModel = userDbModel.Categories.Find(c => c.Id == categoryId);

        if (passwordCategoryDbModel is null)
            throw new PasswordCategoryAccessException($"Category {categoryId} was not found");

        var existingPassword = passwordCategoryDbModel.Passwords.Find(p => p.Id == passwordId);

        if (existingPassword is null)
            throw new PasswordAccessException($"Password {passwordId} was not found");

        existingPassword.IsActive = false;

        await UpdateUserDbModel(userDbModel, cancellationToken);
    }
}
