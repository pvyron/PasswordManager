using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.IServices;

public interface IAuthorizationService
{
    Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    IEnumerable<Claim> SetupUserClaims(UserModel user);
}
