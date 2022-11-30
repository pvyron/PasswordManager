using Bogus;
using LanguageExt.Pipes;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.Services
{
    internal sealed class PasswordService : UsersTableAccessService, IPasswordService
    {
        private const string PASSWORD_TABLE_NAME = "passwords";

        private readonly MDbClient _dbClient;
        private readonly IUsersService _usersService;

        public PasswordService(MDbClient dbClient, IUsersService usersService) : base(dbClient)
        {
            _dbClient = dbClient;
            _usersService = usersService;
        }

        public async IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var userDbModel in await _dbClient.GetRecords<UserDbModel>(USER_TABLE_NAME, (nameof(UserDbModel.Id), userId), cancellationToken))
            {
                if ((userDbModel.Categories?.Sum(c => (c.Passwords?.Count ?? 0)) ?? 0) == 0)
                    continue;

                await foreach (var passwordModel in userDbModel.Categories!.ToAsyncEnumerable().SelectMany(c => c.Passwords!.Select(p => new PasswordModel
                {
                    CategoryId = p.CategoryId,
                    Description = p.Description,
                    Id = p.Id,
                    Password = p.Password,
                    Title = p.Title,
                    UserId = p.UserId,
                    Username = p.Username,
                }).ToAsyncEnumerable()))
                {
                    if (passwordModel is null)
                        continue;

                    yield return passwordModel;
                }
            }
        }

        public async Task<PasswordModel> GetPasswordById(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            var passwordDbModel = await GetPasswordDbModel(userId, null, id, cancellationToken);

            return new PasswordModel
            {
                CategoryId = passwordDbModel.CategoryId,
                Description = passwordDbModel.Description,
                Id = passwordDbModel.Id,
                Password = passwordDbModel.Password,
                Title = passwordDbModel.Title,
                UserId = passwordDbModel.UserId,
                Username = passwordDbModel.Username,
            };
        }

        public async Task<PasswordModel> GetPasswordById(Guid userId, Guid categoryId, Guid id, CancellationToken cancellationToken)
        {
            var passwordDbModel = await GetPasswordDbModel(userId, categoryId, id, cancellationToken);

            return new PasswordModel
            {
                CategoryId = passwordDbModel.CategoryId,
                Description = passwordDbModel.Description,
                Id = passwordDbModel.Id,
                Password = passwordDbModel.Password,
                Title = passwordDbModel.Title,
                UserId = passwordDbModel.UserId,
                Username = passwordDbModel.Username,
            };
        }

        public async Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken)
        {
            UserDbModel userDbModel = await GetUserDbModel(password.UserId, cancellationToken);

            PasswordCategoryDbModel? categoryDbModel;

            if (password.CategoryId is not null)
            {
                categoryDbModel = userDbModel.Categories?.Find(c => c.Id == password.CategoryId);

                throw new PasswordCategoryAccessException($"Category {password.CategoryId} was not found");
            }
            else
            {
                categoryDbModel = userDbModel.Categories?.Find(c => c.Title == "Default");
            }

            categoryDbModel ??= new()
            {
                Id = Guid.NewGuid(),
                Title = "Default",
                IsActive = true,
                UserId = userDbModel.Id,
                Passwords = new()
            };

            var newPasswordDbModel = new PasswordDbModel
            {
                Id = Guid.NewGuid(),
                UserId = userDbModel.Id,
                CategoryId = categoryDbModel.Id,
                Description = password.Description,
                Password = password.Password,
                Title = password.Title,
                Username = password.Username,
            };

            await UpdateUserDbModel(userDbModel, cancellationToken);

            return new PasswordModel
            {
                CategoryId = newPasswordDbModel.CategoryId,
                Description = newPasswordDbModel.Description,
                Id = newPasswordDbModel.Id,
                Password = newPasswordDbModel.Password,
                Title = newPasswordDbModel.Title,
                UserId = newPasswordDbModel.UserId,
                Username = newPasswordDbModel.Username,
            };
        }

        public async Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
        {
            var userDbModel = await GetUserDbModel(password.UserId, cancellationToken);

            var existingPassword = userDbModel.Categories!.Find(c => c.Id == (Guid)password.CategoryId!)!.Passwords!.Find(p => p.Id == password.Id);

            if (existingPassword is null)
                throw new PasswordAccessException("Password was not found", password);

            existingPassword.Description = password.Description;
            existingPassword.Username = password.Username;
            existingPassword.Password = password.Password;
            existingPassword.UserId = password.UserId;
            existingPassword.Username = password.Username;
            existingPassword.CategoryId = password.CategoryId;
            existingPassword.Title = password.Title;
            existingPassword.Password = password.Password;

            await UpdateUserDbModel(userDbModel, cancellationToken);

            return new PasswordModel
            {
                CategoryId = existingPassword.CategoryId,
                Description = existingPassword.Description,
                Id = existingPassword.Id,
                Password = existingPassword.Password,
                Title = existingPassword.Title,
                UserId = existingPassword.UserId,
                Username = existingPassword.Username,
            };
        }

        public async Task DeletePassword(Guid userId, Guid categoryId, Guid id, CancellationToken cancellationToken)
        {
            var passwordDbModel = await GetPasswordDbModel(userId, categoryId, id, cancellationToken);

            passwordDbModel.IsActive = false;

            var userDbModel = await GetUserDbModel(userId, cancellationToken);

            var existingPassword = userDbModel.Categories!.Find(c => c.Id == categoryId)!.Passwords!.Find(p => p.Id == id);

            existingPassword = passwordDbModel;

            await UpdateUserDbModel(userDbModel, cancellationToken);

            await _dbClient.UpdateRecord(PASSWORD_TABLE_NAME, id, passwordDbModel, cancellationToken);
        }

        public async Task CreateRandomPasswords(int numberOfPasswords, CancellationToken cancellationToken)
        {
            var userIds = await _usersService.GetAllUsers(cancellationToken).Select(u => u.Id).ToListAsync(cancellationToken);

            var passwordDbModels = new Faker<PasswordDbModel>()
                .RuleFor(p => p.CategoryId, _ => null)
                .RuleFor(p => p.Title, f => f.Random.Word())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.Username, f => f.Random.Word())
                .RuleFor(p => p.Password, f => f.Internet.Password())
                .RuleFor(p => p.UserId, f => f.PickRandom(userIds))
                .GenerateBetween(numberOfPasswords, numberOfPasswords);

            foreach (var pwModel in passwordDbModels)
            {
                await _dbClient.InsertRecord(PASSWORD_TABLE_NAME, pwModel, cancellationToken);
            }
        }

        private static PasswordDbModel GetPasswordDbModel(UserDbModel userDbModel, Guid id)
        {
            if ((userDbModel.Categories?.Sum(c => (c.Passwords?.Count ?? 0)) ?? 0) == 0)
                throw new PasswordAccessException($"Password {id} was not found");

            var passwordDbModel = userDbModel.Categories?.Find(c => c.Passwords?.Any(p => p.Id == id) ?? false)?.Passwords!.First(p => p.Id == id);

            if (passwordDbModel is null)
                throw new PasswordAccessException($"Password {id} was not found");

            return passwordDbModel;
        }

        private async Task<PasswordDbModel> GetPasswordDbModel(Guid userId, Guid? categoryId, Guid id, CancellationToken cancellationToken)
        {
            var userDbModel = await _dbClient.GetRecordById<UserDbModel>(USER_TABLE_NAME, userId, cancellationToken);

            if (userDbModel is null)
                throw new UserAccessException($"User {userId} was not found");

            if (categoryId is null)
                return GetPasswordDbModel(userDbModel, id);

            var categoryDbModel = userDbModel.Categories?.Find(c => c.Id == categoryId);

            if (categoryDbModel is null)
                throw new PasswordCategoryAccessException($"Category {categoryId} was not found");

            var passwordDbModel = categoryDbModel.Passwords?.Find(p => p.Id == id);

            if (passwordDbModel is null)
                throw new PasswordAccessException($"Password {id} was not found");

            return passwordDbModel;
        }
    }
}
