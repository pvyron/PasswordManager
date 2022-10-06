using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManager.Application.IServices;
using PasswordManager.Application.Models;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;

namespace PasswordManager.Application.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> CreateUser(User user, CancellationToken cancellationToken)
        {
            try
            {
                UserDbModel userDbModel = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                UserDbModel createdUserDbModel = await _unitOfWork.Users.CreateAsync(userDbModel, cancellationToken);

                await _unitOfWork.CompleteAsync();

                User newUser = new()
                {
                    Id = createdUserDbModel.Id,
                    FirstName = createdUserDbModel.FirstName,
                    LastName = createdUserDbModel.LastName,
                    Email = createdUserDbModel.Email
                };

                return newUser;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
