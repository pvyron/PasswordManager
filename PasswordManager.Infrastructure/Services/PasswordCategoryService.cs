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
    internal sealed class PasswordCategoryService : UsersTableAccessService, IPasswordCategoriesService
    {
        public PasswordCategoryService(MDbClient dbClient) : base(dbClient)
        {
        }

        public async IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var user = await GetUserDbModel(userId, cancellationToken);

            foreach (var passwordCategoryDbModel in user.Categories)
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

        public async Task<PasswordCategoryModel> GetCategoryById(Guid userId, Guid categoryId, CancellationToken cancellationToken)
        {
            var user = await GetUserDbModel(userId, cancellationToken);

            var passwordCategoryDbModel = user.Categories.Find(c => c.Id == categoryId);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password category {categoryId} was not found");

            return new PasswordCategoryModel
            {
                Description = passwordCategoryDbModel.Description,
                Id = passwordCategoryDbModel.Id,
                Title = passwordCategoryDbModel.Title,
                UserId = userId,
            };
        }

        public async Task<PasswordCategoryModel> CreateNewCategory(PasswordCategoryModel passwordCategory, CancellationToken cancellationToken)
        {
            var user = await GetUserDbModel(passwordCategory.UserId, cancellationToken);

            if (user.Categories.Any(c => c.Title == passwordCategory.Title && c.IsActive))
                throw new PasswordCategoryAccessException($"Category with title {passwordCategory.Title} already exists");

            var newPasswordCategoryDbModel = new PasswordCategoryDbModel
            {
                Description = passwordCategory.Description,
                Id = passwordCategory.Id,
                Title = passwordCategory.Title,
            };

            user.Categories.Add(newPasswordCategoryDbModel);

            user = await UpdateUserDbModel(user, cancellationToken);

            newPasswordCategoryDbModel = user.Categories.Find(c => c.Id == newPasswordCategoryDbModel.Id)!;

            return new PasswordCategoryModel
            {
                Id = newPasswordCategoryDbModel.Id,
                Title = newPasswordCategoryDbModel.Title,
                UserId = user.Id,
                Description = newPasswordCategoryDbModel.Description,
            };
        }

        public async Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel passwordCategory, CancellationToken cancellationToken)
        {
            var user = await GetUserDbModel(passwordCategory.UserId, cancellationToken);

            var passwordCategoryDbModel = user.Categories.Find(c => c.Id == passwordCategory.Id);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password Category with id {passwordCategory.Id} was not found", passwordCategory);

            passwordCategoryDbModel.Description = passwordCategory.Description;
            passwordCategoryDbModel.Title = passwordCategory.Title;

            user = await UpdateUserDbModel(user, cancellationToken);

            passwordCategoryDbModel = user.Categories.Find(c => c.Id == passwordCategoryDbModel.Id)!;

            return new PasswordCategoryModel
            {
                Id = passwordCategoryDbModel.Id,
                Title = passwordCategoryDbModel.Title,
                UserId = user.Id,
                Description = passwordCategoryDbModel.Description,
            };
        }

        public async Task DeleteCategory(Guid userId, Guid categoryId, CancellationToken cancellationToken)
        {
            var user = await GetUserDbModel(userId, cancellationToken);

            var passwordCategoryDbModel = user.Categories.Find(c => c.Id == categoryId);

            if (passwordCategoryDbModel is null)
                throw new PasswordCategoryAccessException($"Password Category with id {categoryId} was not found");

            passwordCategoryDbModel.IsActive = false;

            user = await UpdateUserDbModel(user, cancellationToken);
        }
    }
}
