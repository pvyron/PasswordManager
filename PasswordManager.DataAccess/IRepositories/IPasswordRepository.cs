using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.IRepositories
{
    internal interface IPasswordRepository : IRepository<PasswordDbModel>
    {
    }
}
