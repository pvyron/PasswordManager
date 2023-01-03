using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.Domain.Models;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.Services;

internal sealed class PasswordCategoryService : IPasswordCategoriesService
{
    private readonly AzureMainDatabaseContext _context;

    public PasswordCategoryService(AzureMainDatabaseContext context)
    {
        _context = context;
    }

    public Task<PasswordCategoryModel> CreateNewCategory(PasswordCategoryModel password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategory(Guid userId, Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var category in _context.PasswordCategories.Where(p => p.User!.Id == userId).ToAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return new PasswordCategoryModel
            {
                Id = category.Id,
                Description = category.Description,
                Title = category.Title,
                UserId = userId,
            };
        }
    }

    public Task<PasswordCategoryModel> GetCategoryById(Guid userId, Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
