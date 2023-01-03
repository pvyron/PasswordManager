using Bogus;
using LanguageExt.Pipes;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System.Runtime.CompilerServices;

namespace PasswordManager.Infrastructure.Services;

internal sealed class PasswordService : IPasswordService
{
    private readonly AzureMainDatabaseContext _context;

    public PasswordService(AzureMainDatabaseContext context)
    {
        _context = context;
    }

    public Task DeletePassword(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, CancellationToken cancellationToken)
    {
        foreach( var password in await _context.Passwords.Where(p => p.User!.Id == userId).ToListAsync(cancellationToken))
        {
            yield return new PasswordModel
            {
                Id= password.Id,
                CategoryId = password.CategoryId,
                Description= password.Description,
                Password = password.Password,
                Title= password.Title,
                UserId= userId,
                Username= password.Username,
            };
        }
    }

    public Task<PasswordModel> GetPasswordById(Guid userId, Guid passwordId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordModel> GetPasswordById(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken)
    {
        var addPasswordResult = await _context.Passwords.AddAsync(new PasswordDbModel
        {
            CategoryId = password.CategoryId!.Value,
            Description = password.Description,
            Password = password.Password,
            Title = password.Title,
            Username = password.Username,
            UserId = password.UserId
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new PasswordModel
        {
            Id = addPasswordResult.Entity.Id,
            CategoryId = addPasswordResult.Entity.CategoryId,
            Description = addPasswordResult.Entity.Description,
            Password = addPasswordResult.Entity.Password,
            Title = addPasswordResult.Entity.Title,
            UserId = addPasswordResult.Entity.UserId,
            Username = addPasswordResult.Entity.Username,
        }; 
    }

    public Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
