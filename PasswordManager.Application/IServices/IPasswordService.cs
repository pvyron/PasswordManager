using PasswordManager.Domain.Models;

namespace PasswordManager.Application.IServices
{
    public interface IPasswordService
    {
        IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, CancellationToken cancellationToken);
        Task<PasswordModel> GetPasswordById(Guid userId, Guid passwordId, CancellationToken cancellationToken);
        Task<PasswordModel> GetPasswordById(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken);
        Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken);
        Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken);
        Task DeletePassword(Guid userId, Guid categoryId, Guid passwordId, CancellationToken cancellationToken);
    }
}
