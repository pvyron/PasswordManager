using Microsoft.EntityFrameworkCore;
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

    public async IAsyncEnumerable<PasswordCategoryModel> GetAllUserCategories(Guid userId, CancellationToken cancellationToken)
    {
        foreach (var category in await _context.PasswordCategories.Where(p => p.User!.Id == userId).ToListAsync(cancellationToken))
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
