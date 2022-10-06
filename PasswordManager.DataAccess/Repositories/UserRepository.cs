using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<UserDbModel> CreateAsync(UserDbModel entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserDbModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDbModel?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDbModel?> UpdateAsync(UserDbModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
