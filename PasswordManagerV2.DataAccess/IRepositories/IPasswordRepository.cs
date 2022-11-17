using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.IRepositories
{
    public interface IPasswordRepository : IRepository<PasswordDbModel>
    {
    }
}
