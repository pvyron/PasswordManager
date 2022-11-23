using LanguageExt.Pipes;
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

namespace PasswordManager.Infrastructure.Services
{
    internal sealed class PasswordService : IPasswordService
    {
        private const string PASSWORD_TABLE_NAME = "passwords";

        private readonly MDbClient _dbClient;
        private readonly IUsersService _usersService;

        public PasswordService(MDbClient dbClient, IUsersService usersService)
        {
            _dbClient = dbClient;
            _usersService = usersService;
        }

        public async IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var passwordDbModel in await _dbClient.GetRecords<PasswordDbModel>(PASSWORD_TABLE_NAME, (nameof(PasswordDbModel.UserId), userId), cancellationToken))
            {
                yield return new PasswordModel
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
        }

        public async Task<PasswordModel> GetPasswordById(Guid id, CancellationToken cancellationToken)
        {
            var passwordDbModel = await _dbClient.GetRecordById<PasswordDbModel>(PASSWORD_TABLE_NAME, id, cancellationToken);

            if (passwordDbModel is null)
                throw new PasswordAccessException($"Password {id} was not found");

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
            _ = await _usersService.GetUserById(password.UserId, cancellationToken);

            var newPasswordDbModel = new PasswordDbModel
            {
                UserId = password.UserId,
                CategoryId = password.CategoryId,
                Description = password.Description,
                Password = password.Password,
                Title = password.Title,
                Username = password.Username,
            };

            var createdPassword = await _dbClient.InsertRecord(PASSWORD_TABLE_NAME, newPasswordDbModel, cancellationToken);

            return new PasswordModel
            {
                CategoryId = newPasswordDbModel.CategoryId,
                Description = newPasswordDbModel.Description,
                Id = createdPassword.Id,
                Password = newPasswordDbModel.Password,
                Title = newPasswordDbModel.Title,
                UserId = newPasswordDbModel.UserId,
                Username = newPasswordDbModel.Username,
            };
        }

        public async Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
        {
            var passwordDbModel = await _dbClient.GetRecordById<PasswordDbModel?>(PASSWORD_TABLE_NAME, password.Id, cancellationToken);

            if (passwordDbModel is null)
                throw new PasswordAccessException($"Password with id {password.Id} was not found", password);

            passwordDbModel.Description = password.Description;
            passwordDbModel.Username = password.Username;
            passwordDbModel.Password = password.Password;
            passwordDbModel.UserId = password.UserId;
            passwordDbModel.Username = password.Username;
            passwordDbModel.CategoryId = password.CategoryId;
            passwordDbModel.Title = password.Title;
            passwordDbModel.Password = password.Password;

            var updatedPasswordDbModel = await _dbClient.UpdateRecord(PASSWORD_TABLE_NAME, password.Id, passwordDbModel, cancellationToken);

            return new PasswordModel
            {
                CategoryId = updatedPasswordDbModel.CategoryId,
                Description = updatedPasswordDbModel.Description,
                Id = updatedPasswordDbModel.Id,
                Password = updatedPasswordDbModel.Password,
                Title = updatedPasswordDbModel.Title,
                UserId = updatedPasswordDbModel.UserId,
                Username = updatedPasswordDbModel.Username,
            };
        }

        public async Task DeletePassword(Guid id, CancellationToken cancellationToken)
        {
            var passwordDbModel = await _dbClient.GetRecordById<PasswordDbModel?>(PASSWORD_TABLE_NAME, id, cancellationToken);

            if (passwordDbModel is null)
                throw new PasswordAccessException($"Password with id {id} was not found");

            passwordDbModel.IsActive = false;

            await _dbClient.UpdateRecord(PASSWORD_TABLE_NAME, id, passwordDbModel, cancellationToken);
        }
    }
}
