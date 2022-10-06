using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.DataAccess.IRepositories
{
    internal interface IUserRepository : IRepository<UserDbModel>
    {
    }
}
