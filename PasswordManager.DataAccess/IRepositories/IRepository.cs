using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.IRepositories
{
    internal interface IRepository<T> where T : IDbModel
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
