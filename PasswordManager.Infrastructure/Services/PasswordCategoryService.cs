using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.Services;

internal sealed class PasswordCategoryService : IPasswordCategoriesService
{
    private readonly ISqlDbContext _context;

    public PasswordCategoryService(ISqlDbContext context)
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
        await foreach (var category in _context.PasswordCategories.Where(c => c.UserId == userId && c.IsActive).ToAsyncEnumerable().WithCancellation(cancellationToken))
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

    public async Task<PasswordCategoryModel> GetCategoryById(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _context.PasswordCategories.FirstOrDefaultAsync(c => c.Id == categoryId && c.IsActive, cancellationToken);

        if (category is null)
        {
            throw new PasswordCategoryAccessException($"Category with id {categoryId} was not found");
        }

        return new PasswordCategoryModel
        {
            Description = category.Description,
            Title = category.Title,
            UserId = category.UserId!.Value,
            Id = category.Id
        };
    }

    public Task<PasswordCategoryModel> UpdateCategory(PasswordCategoryModel password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
