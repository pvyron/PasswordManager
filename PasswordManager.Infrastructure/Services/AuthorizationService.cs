using LanguageExt.Pipes;
using LanguageExt.Pretty;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess;
using PasswordManager.DataAccess.DbModels;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.Services;

public sealed class AuthorizationService : IAuthorizationService
{
    private const int DAYS_TOKEN_ACTIVE = 15;
    private const string USER_TABLE_NAME = "users";

    private DateTime _tokenExpirationDate => DateTime.UtcNow.AddDays(DAYS_TOKEN_ACTIVE);
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _jwtSignKey;
    private readonly MDbClient _dbClient;

    public AuthorizationService(MDbClient mDbClient, IConfiguration configuration)
    {
        _dbClient = mDbClient;
        _issuer = configuration.GetValue<string>("AuthenticationServiceSettings:JwtIssuer")!;
        _jwtSignKey = configuration.GetValue<string>("AuthenticationServiceSettings:JwtKey")!;
        _audience = configuration.GetValue<string>("AuthenticationServiceSettings:JwtAudience")!;
    }

    public async Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken)
    {
        var userDbModel = await _dbClient.GetRecord<UserDbModel>(USER_TABLE_NAME, ("Email", email), cancellationToken);

        if (userDbModel is null || password != userDbModel.Password) { throw new AuthenticationException("Invalid credentials"); }

        return new UserModel
        {
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
        var key = Encoding.ASCII.GetBytes(_jwtSignKey);
        var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            expires: _tokenExpirationDate,
            claims: claims,
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
