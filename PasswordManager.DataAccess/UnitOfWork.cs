using PasswordManager.DataAccess;
using PasswordManager.DataAccess.IRepositories;
using PasswordManager.DataAccess.Repositories;

namespace PasswordManager.DataAccess
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IPasswordRepository PasswordRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            PasswordRepository = new PasswordRepository(_dbContext);
            UserRepository = new UserRepository(_dbContext);
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