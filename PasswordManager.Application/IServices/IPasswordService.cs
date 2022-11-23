using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.IServices
{
    public interface IPasswordService
    {
        IAsyncEnumerable<PasswordModel> GetAllUserPasswords(Guid userId, CancellationToken cancellationToken);
        Task<PasswordModel> GetPasswordById(Guid id, CancellationToken cancellationToken);
        Task<PasswordModel> SaveNewPassword(PasswordModel password, CancellationToken cancellationToken);
        Task<PasswordModel> UpdatePassword(PasswordModel password, CancellationToken cancellationToken);
        Task DeletePassword(Guid id, CancellationToken cancellationToken);
    }
}
