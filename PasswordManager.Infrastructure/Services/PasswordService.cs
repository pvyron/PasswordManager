using LanguageExt;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.Interfaces;
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
        await foreach (var passwordDbModel in _context.Passwords.Include(p => p.Image).Where(p => p.UserId == userId && p.IsActive).AsAsyncEnumerable().WithCancellation(cancellationToken))
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
                ImageId = passwordDbModel.ImageId,
                Logo = new PasswordLogoModel
                {
                    LogoId = passwordDbModel.Image!.Id,
                    Title = passwordDbModel.Image!.Title,
                    FileExtension = "jpg",
                    FileUrl = passwordDbModel.Image!.ImageUrl
                }
            };
        }
    }

    public async Task<PasswordModel> GetPasswordById(Guid passwordId, CancellationToken cancellationToken)
    {
        var passwordDbModel = await _context.Passwords.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == passwordId && p.IsActive, cancellationToken);

        if (passwordDbModel is null)
            throw new PasswordAccessException($"Password with id {passwordId} was not found");

        return new PasswordModel
        {
            CategoryId = passwordDbModel.CategoryId,
            Description = passwordDbModel.Description,
            Password = passwordDbModel.Password,
            Title = passwordDbModel.Title,
            UserId = passwordDbModel.UserId!.Value,
            Id = passwordDbModel.Id,
            Username = passwordDbModel.Username,
            IsFavorite = passwordDbModel.IsFavorite,
            ImageId = passwordDbModel.ImageId,
            Logo = new PasswordLogoModel
            {
                LogoId = passwordDbModel.Image!.Id,
                Title = passwordDbModel.Image!.Title,
                FileExtension = "jpg",
                FileUrl = passwordDbModel.Image!.ImageUrl
            }
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
            IsFavorite = password.IsFavorite.GetValueOrDefault(false),
            ImageId = password.ImageId,
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var passwordDbModel = addPasswordResult.Entity;
        passwordDbModel.Image = await _context.PasswordLogos.FindAsync(new object[] { passwordDbModel.ImageId.GetValueOrDefault() }, cancellationToken: cancellationToken);

        return new PasswordModel
        {
            CategoryId = passwordDbModel.CategoryId,
            Description = passwordDbModel.Description,
            Password = passwordDbModel.Password,
            Title = passwordDbModel.Title,
            UserId = passwordDbModel.UserId!.Value,
            Id = passwordDbModel.Id,
            Username = passwordDbModel.Username,
            IsFavorite = passwordDbModel.IsFavorite,
            ImageId = passwordDbModel.ImageId,
            Logo = new PasswordLogoModel
            {
                LogoId = passwordDbModel.Image!.Id,
                Title = passwordDbModel.Image!.Title,
                FileExtension = "jpg",
                FileUrl = passwordDbModel.Image!.ImageUrl
            }
        };
    }

    public async Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken)
    {
        var passwordDbModel = await _context.Passwords.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == password.Id && p.IsActive, cancellationToken);

        passwordDbModel!.CategoryId = password.CategoryId!.Value;
        passwordDbModel!.Description = password.Description;
        passwordDbModel!.Title = password.Title;
        passwordDbModel!.Username = password.Username;
        passwordDbModel!.Password = password.Password;
        passwordDbModel!.IsFavorite = password.IsFavorite.GetValueOrDefault(false);
        passwordDbModel!.ImageId = password.ImageId;
        passwordDbModel!.EditedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new PasswordModel
        {
            CategoryId = passwordDbModel.CategoryId,
            Description = passwordDbModel.Description,
            Password = passwordDbModel.Password,
            Title = passwordDbModel.Title,
            UserId = passwordDbModel.UserId!.Value,
            Id = passwordDbModel.Id,
            Username = passwordDbModel.Username,
            IsFavorite = passwordDbModel.IsFavorite,
            ImageId = passwordDbModel.ImageId,
            Logo = new PasswordLogoModel
            {
                LogoId = passwordDbModel.Image!.Id,
                Title = passwordDbModel.Image!.Title,
                FileExtension = "jpg",
                FileUrl = passwordDbModel.Image!.ImageUrl
            }
        };
    }

    public async Task<PasswordModel> FavoritePassword(Guid id, bool isFavorite, CancellationToken cancellationToken)
    {
        var passwordDbModel = await _context.Passwords.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        passwordDbModel!.IsFavorite = isFavorite;
        passwordDbModel!.EditedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new PasswordModel
        {
            Id = passwordDbModel.Id,
            CategoryId = passwordDbModel.CategoryId,
            Description = passwordDbModel.Description,
            Password = passwordDbModel.Password,
            Title = passwordDbModel.Title,
            UserId = passwordDbModel.UserId!.Value,
            Username = passwordDbModel.Username,
            IsFavorite = passwordDbModel.IsFavorite,
            ImageId = passwordDbModel.ImageId,
            Logo = new PasswordLogoModel
            {
                LogoId = passwordDbModel.Image!.Id,
                Title = passwordDbModel.Image!.Title,
                FileExtension = "jpg",
                FileUrl = passwordDbModel.Image!.ImageUrl,
            }
        }; ;
    }

    public async Task DeletePassword(Guid passwordId, CancellationToken cancellationToken)
    {
        var passwordToDelete = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == passwordId, cancellationToken);

        passwordToDelete!.EditedAt = DateTimeOffset.UtcNow;
        passwordToDelete!.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
