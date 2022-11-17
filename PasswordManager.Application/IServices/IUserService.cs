using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
