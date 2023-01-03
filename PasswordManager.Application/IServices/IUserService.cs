using PasswordManager.Domain.Models;

namespace PasswordManager.Application.IServices;

public interface IUsersService
{
    IAsyncEnumerable<UserModel> GetAllUsers(CancellationToken cancellationToken);
    Task<UserModel> GetUserById(Guid id, CancellationToken cancellationToken);
    Task<UserModel> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<UserModel> CreateUser(UserModel user, string password, CancellationToken cancellationToken);
    Task<UserModel> UpdateUser(UserModel user, CancellationToken cancellationToken);
    Task<UserModel> UpdateUserPassword(Guid id, string newPassword, CancellationToken cancellationToken);
    Task DeleteUser(Guid id, CancellationToken cancellationToken);
    Task PopulateDb(CancellationToken cancellationToken);
}
