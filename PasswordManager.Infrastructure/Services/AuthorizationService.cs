using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using PasswordManager.Infrastructure.ServiceSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PasswordManager.Infrastructure.Services;

internal sealed class AuthorizationService : IAuthorizationService
{
    private const int DAYS_TOKEN_ACTIVE = 15;

    private static DateTime TokenExpirationDate => DateTime.UtcNow.AddDays(DAYS_TOKEN_ACTIVE);

    private readonly AzureMainDatabaseContext _context;
    private readonly AuthorizationServiceSettings _settings;

    public AuthorizationService(IOptions<AuthorizationServiceSettings> options, AzureMainDatabaseContext context)
    {
        _settings = options.Value;
        _settings.Validate();

        _context = context;
    }

    public async Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken)
    {
        var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userDbModel is null || password != userDbModel.Password) { throw new AuthenticationException("Invalid credentials"); }

        return new UserModel
        {
            Id = userDbModel.Id,
            Email = userDbModel.Email,
            FirstName = userDbModel.FirstName,
            LastName = userDbModel.LastName,
        };
    }

    public IEnumerable<Claim> SetupUserClaims(UserModel user)
    {
        var claims = new Claim[]
        {
            new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
            new Claim(CustomClaimTypes.UserEmail, user.Email),
            new Claim(CustomClaimTypes.FullName, user.FullName)
        };

        return claims;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(_settings.JwtKey);
        var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var jwtToken = new JwtSecurityToken(
            issuer: _settings.JwtIssuer,
            audience: _settings.JwtAudience,
            expires: TokenExpirationDate,
            claims: claims,
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
