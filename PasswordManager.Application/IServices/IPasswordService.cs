using PasswordManager.Domain.Models;

namespace PasswordManager.Application.IServices
{
    public interface IPasswordService
    {
        IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, CancellationToken cancellationToken);
        Task<PasswordModel> GetPasswordById(Guid passwordId, CancellationToken cancellationToken);
        Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken);
        Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken);
        Task DeletePassword(Guid passwordId, CancellationToken cancellationToken);
        Task<PasswordModel> FavoritePassword(Guid id, bool isFavorite, CancellationToken cancellationToken);
    }
}
