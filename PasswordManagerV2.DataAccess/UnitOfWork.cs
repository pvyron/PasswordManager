using PasswordManager.DataAccess;
using PasswordManager.DataAccess.IRepositories;
using PasswordManager.DataAccess.Repositories;

namespace PasswordManager.DataAccess
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IPasswordRepository Passwords { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Passwords = new PasswordRepository(_dbContext);
            Users = new UserRepository(_dbContext);
        }

        public Task BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public Task CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransaction()
        {
            throw new NotImplementedException();
        }
    }
}