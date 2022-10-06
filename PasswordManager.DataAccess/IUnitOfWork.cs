using PasswordManager.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess
{
    public interface IUnitOfWork
    {
        IPasswordRepository Passwords { get; }
        IUserRepository Users { get; }

        Task CompleteAsync();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
