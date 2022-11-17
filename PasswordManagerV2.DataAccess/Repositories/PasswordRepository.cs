using PasswordManager.DataAccess.DbModels;
using PasswordManager.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataAccess.Repositories
{
    internal sealed class PasswordRepository : IPasswordRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<PasswordDbModel> CreateAsync(PasswordDbModel entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PasswordDbModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDbModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDbModel?> UpdateAsync(PasswordDbModel entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
