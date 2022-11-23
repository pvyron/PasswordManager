using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.IServices;

public interface IPasswordCategoriesService
{
    IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> GetCategoryById(Guid id, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> CreateNewCategory(PasswordCategoryModel password, CancellationToken cancellationToken);
    Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel password, CancellationToken cancellationToken);
    Task DeleteCategory(Guid id, CancellationToken cancellationToken);
}
