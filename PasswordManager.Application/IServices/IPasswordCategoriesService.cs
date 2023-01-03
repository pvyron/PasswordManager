using PasswordManager.Domain.Models;

namespace PasswordManager.Application.IServices;

public interface IPasswordCategoriesService
{
    IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> GetCategoryById(Guid categoryId, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> CreateNewCategory(PasswordCategoryModel password, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel password, CancellationToken cancellationToken);
    Task DeleteCategory(Guid userId, Guid categoryId, CancellationToken cancellationToken);
}
