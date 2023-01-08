using LanguageExt;
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
    private readonly ISqlDbContext _context;

    public PasswordService(ISqlDbContext context)
    {
        _context = context;
    }

    public async IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var passwordDbModel in _context.Passwords.Where(p => p.UserId == userId && p.IsActive).AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return new PasswordModel
            {
                Id = passwordDbModel.Id,
                CategoryId = passwordDbModel.CategoryId,
                Description = passwordDbModel.Description,
                Password = passwordDbModel.Password,
                Title = passwordDbModel.Title,
                UserId = passwordDbModel.UserId!.Value,
                Username = passwordDbModel.Username,
                IsFavorite = passwordDbModel.IsFavorite,
            };
        }
    }

    public async Task<PasswordModel> GetPasswordById(Guid passwordId, CancellationToken cancellationToken)
    {
        var passwordResult = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == passwordId && p.IsActive, cancellationToken);

        if (passwordResult is null)
            throw new PasswordAccessException($"Password with id {passwordId} was not found");

        return new PasswordModel
        {
            CategoryId = passwordResult.CategoryId,
            Description = passwordResult.Description,
            Password = passwordResult.Password,
            Title = passwordResult.Title,
            UserId = passwordResult.UserId!.Value,
            Id = passwordResult.Id,
            Username = passwordResult.Username,
            IsFavorite = passwordResult.IsFavorite,
        };
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
            UserId = password.UserId,
            IsFavorite = password.IsFavorite,
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new PasswordModel
        {
            Id = addPasswordResult.Entity.Id,
            CategoryId = addPasswordResult.Entity.CategoryId,
            Description = addPasswordResult.Entity.Description,
            Password = addPasswordResult.Entity.Password,
            Title = addPasswordResult.Entity.Title,
            UserId = addPasswordResult.Entity.UserId!.Value,
            Username = addPasswordResult.Entity.Username,
            IsFavorite = addPasswordResult.Entity.IsFavorite,
        };
    }

    public async Task UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
    {
        var passwordToUpdate = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == password.Id && p.IsActive, cancellationToken);

        passwordToUpdate!.CategoryId = password.CategoryId!.Value;
        passwordToUpdate!.Description = password.Description;
        passwordToUpdate!.Title = password.Title;
        passwordToUpdate!.Username = password.Username;
        passwordToUpdate!.Password = password.Password;
        passwordToUpdate!.IsFavorite = password.IsFavorite;
        passwordToUpdate!.EditedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordModel> FavoritePassword(Guid id, bool isFavorite, CancellationToken cancellationToken)
    {
        var passwordToUpdate = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        passwordToUpdate!.IsFavorite = isFavorite;
        passwordToUpdate!.EditedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new PasswordModel
        {
            Id = passwordToUpdate.Id,
            IsFavorite = passwordToUpdate.IsFavorite,
            CategoryId = passwordToUpdate.CategoryId,
            Description = passwordToUpdate.Description,
            Password = passwordToUpdate.Password,
            Title = passwordToUpdate.Title,
            UserId = passwordToUpdate.UserId!.Value,
            Username = passwordToUpdate.Username,
        };
    }

    public async Task DeletePassword(Guid passwordId, CancellationToken cancellationToken)
    {
        var passwordToDelete = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == passwordId, cancellationToken);

        passwordToDelete!.EditedAt = DateTimeOffset.UtcNow;
        passwordToDelete!.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
