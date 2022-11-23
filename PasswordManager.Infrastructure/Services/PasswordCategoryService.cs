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
    internal sealed class PasswordCategoryService : IPasswordCategoriesService
    {
        private const string PASSWORD_CATEGORY_TABLE_NAME = "pass_categories";

        private readonly MDbClient _dbClient;
        private readonly IUsersService _usersService;

        public PasswordCategoryService(MDbClient dbClient, IUsersService usersService)
        {
            _dbClient = dbClient;
            _usersService = usersService;
        }

        public async IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var passwordCategoryDbModel in await _dbClient.GetRecords<PasswordCategoryDbModel>(PASSWORD_CATEGORY_TABLE_NAME, (nameof(PasswordCategoryDbModel.UserId), userId), cancellationToken))
            {
                yield return new PasswordCategoryModel
                {
                    Description = passwordCategoryDbModel.Description,
                    Id = passwordCategoryDbModel.Id,
                    Title = passwordCategoryDbModel.Title,
                    UserId = userId,
                };
            }
        }

        public async Task<PasswordCategoryModel> GetCategoryById(Guid id, CancellationToken cancellationToken)
        {
            var passwordCategoryDbModel = await _dbClient.GetRecordById<PasswordCategoryDbModel>(PASSWORD_CATEGORY_TABLE_NAME, id, cancellationToken);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password category {id} was not found");

            return new PasswordCategoryModel
            {
                Description = passwordCategoryDbModel.Description,
                Id = passwordCategoryDbModel.Id,
                Title = passwordCategoryDbModel.Title,
                UserId = passwordCategoryDbModel.UserId,
            };
        }

        public async Task<PasswordCategoryModel> CreateNewCategory(PasswordCategoryModel passwordCategory, CancellationToken cancellationToken)
        {
            _ = await _usersService.GetUserById(passwordCategory.UserId, cancellationToken);

            var newPasswordCategoryDbModel = new PasswordCategoryDbModel
            {
                Description = passwordCategory.Description,
                Id = passwordCategory.Id,
                Title = passwordCategory.Title,
                UserId = passwordCategory.UserId,
            };

            var createdPasswordCategoryDbModel = await _dbClient.InsertRecord(PASSWORD_CATEGORY_TABLE_NAME, newPasswordCategoryDbModel, cancellationToken);

            return new PasswordCategoryModel
            {
                Id = createdPasswordCategoryDbModel.Id,
                Title = createdPasswordCategoryDbModel.Title,
                UserId = createdPasswordCategoryDbModel.UserId,
                Description = createdPasswordCategoryDbModel.Description,
            };
        }

        public async Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel passwordCategory, CancellationToken cancellationToken)
        {
            var passwordCategoryDbModel = await _dbClient.GetRecordById<PasswordCategoryDbModel?>(PASSWORD_CATEGORY_TABLE_NAME, passwordCategory.Id, cancellationToken);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password Category with id {passwordCategory.Id} was not found", passwordCategory);

            passwordCategoryDbModel.Description = passwordCategory.Description;
            passwordCategoryDbModel.Title = passwordCategory.Title;

            var updatedPasswordCategoryDbModel = await _dbClient.UpdateRecord(PASSWORD_CATEGORY_TABLE_NAME, passwordCategory.Id, passwordCategoryDbModel, cancellationToken);

            return new PasswordCategoryModel
            {
                Id = updatedPasswordCategoryDbModel.Id,
                Title = updatedPasswordCategoryDbModel.Title,
                UserId = updatedPasswordCategoryDbModel.UserId,
                Description = updatedPasswordCategoryDbModel.Description,
            };
        }

        public async Task DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            var passwordCategoryDbModel = await _dbClient.GetRecordById<PasswordCategoryDbModel?>(PASSWORD_CATEGORY_TABLE_NAME, id, cancellationToken);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password Category with id {id} was not found");

            passwordCategoryDbModel.IsActive = false;

            await _dbClient.UpdateRecord(PASSWORD_CATEGORY_TABLE_NAME, id, passwordCategoryDbModel, cancellationToken);
        }
    }
}
