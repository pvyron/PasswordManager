using PasswordManager.Domain.Models;
using System.Security.Claims;

namespace PasswordManager.Application.IServices;

public interface IAuthorizationService
{
    Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    IEnumerable<Claim> SetupUserClaims(UserModel user);
}
