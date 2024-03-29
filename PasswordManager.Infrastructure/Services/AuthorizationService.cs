﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.DataAccess.Interfaces;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using PasswordManager.Infrastructure.ServiceSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Infrastructure.Services;

internal sealed class AuthorizationService : IAuthorizationService
{
    private const int DAYS_TOKEN_ACTIVE = 15;

    private static DateTime TokenExpirationDate => DateTime.UtcNow.AddDays(DAYS_TOKEN_ACTIVE);

    private readonly ISqlDbContext _context;
    private readonly AuthorizationServiceSettings _settings;
    private readonly RandomNumberGenerator _randomNumberGenerator;

    public AuthorizationService(IOptions<AuthorizationServiceSettings> options, ISqlDbContext context)
    {
        _settings = options.Value;
        _settings.Validate();

        _context = context;
        _randomNumberGenerator = RandomNumberGenerator.Create();
    }

    public async Task<UserModel> Authenticate(string email, string password, CancellationToken cancellationToken)
    {
        var userDbModel = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (userDbModel is null || !await ValidatePassword(HashedPassword(password, userDbModel.PasswordSalt), userDbModel.Password)) { throw new AuthenticationException("Invalid credentials"); }

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

    public (string hashedPassword, byte[] privateSalt) GenerateHashedPassword(string plainPassword)
    {
        var privateSalt = new byte[32];

        _randomNumberGenerator.GetBytes(privateSalt);

        return (GenerateHashedPassword(plainPassword, privateSalt), privateSalt);
    }

    public string GenerateHashedPassword(string plainPassword, byte[] privateSalt)
    {
        return HashedPassword(plainPassword, privateSalt);
    }

    string HashedPassword(string plainPassword, byte[] privateSalt)
    {
        var publicSalt = _settings.PublicPasswordHashingSalt;
        var publicSaltBytes = Encoding.UTF8.GetBytes(publicSalt);
        var plainPasswordBytes = Encoding.UTF8.GetBytes(plainPassword);

        byte[] hashingInput = publicSaltBytes.Append(plainPasswordBytes).Append(privateSalt).ToArray();

        var computedHash = SHA256.HashData(hashingInput);

        var hashedPasword = Convert.ToBase64String(computedHash);

        return hashedPasword;
    }

    public async static Task<bool> ValidatePassword(string inputPassword, string savedPassword)
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(50, 1000));

        if (inputPassword.Length != savedPassword.Length)
        {
            return false;
        }

        var result = 0;
        for (var i = 0; i < inputPassword.Length; i++)
        {
            result |= inputPassword[i] ^ savedPassword[i];
        }

        return result == 0;
    }
}
