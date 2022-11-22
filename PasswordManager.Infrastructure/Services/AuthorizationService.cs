using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

public sealed class AuthorizationService : IAuthorizationService
{
    private const string USER_TABLE_NAME = "users";

    private readonly MDbClient _dbClient;

    public AuthorizationService(MDbClient mDbClient)
    {
        _dbClient = mDbClient;
    }

    public async Task Authenticate(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _dbClient.GetRecord<UserDbModel>(USER_TABLE_NAME, ("Email", email), cancellationToken);

        if (user is null || password != user.Password) { throw new Exception(""); }


    }
}
