using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.IRepositories
{
    public interface IRepository<T> where T : IDbModel
    {
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
