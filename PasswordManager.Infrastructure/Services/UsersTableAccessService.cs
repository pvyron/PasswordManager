using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

abstract class UsersTableAccessService
{
    protected const string USER_TABLE_NAME = "users";
    private readonly MDbClient _dbClient;

    public UsersTableAccessService(MDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    protected async Task<UserDbModel> GetUserDbModel(Guid userId, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecordById<UserDbModel>(USER_TABLE_NAME, userId, cancellationToken);

        if (userDbModel is null)
            throw new UserAccessException($"User {userId} was not found");

        return userDbModel;
    }
}
