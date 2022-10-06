using PasswordManager.Application.Models;

namespace PasswordManager.Application.IServices
{
    public interface IUsersService
    {
        Task<User?> CreateUser(User user, CancellationToken cancellationToken);
    }
}