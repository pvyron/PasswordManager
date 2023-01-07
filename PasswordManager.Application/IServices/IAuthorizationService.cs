using PasswordManager.Domain.Models;
using System.Security.Claims;

namespace PasswordManager.Application.IServices;

public interface IAuthorizationService
{
    (string hashedPassword, byte[] privateSalt) GenerateHashedPassword(string plainPassword);
    string GenerateHashedPassword(string plainPassword, byte[] privateSalt);
    Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    IEnumerable<Claim> SetupUserClaims(UserModel user);
}
