﻿using LanguageExt.Common;
using Mediator;
using PasswordManager.Application.DtObjects.Authorization;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;

namespace PasswordManager.Application.Queries.Authorization;

public sealed record LoginUserQuery(string? Email, string? Password) : IRequest<Result<UserLoginResponseModel>>;

public sealed class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<UserLoginResponseModel>>
{
    private readonly IAuthorizationService _authorizationService;

    public LoginUserQueryHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async ValueTask<Result<UserLoginResponseModel>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new Result<UserLoginResponseModel>(new AuthenticationException("Please provide credentials on the request"));
            }

            var user = await _authorizationService.Authenticate(request.Email, request.Password, cancellationToken);

            var claims = _authorizationService.SetupUserClaims(user);

            var authenticationToken = _authorizationService.GenerateAccessToken(claims);

            return new UserLoginResponseModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = authenticationToken
            };
        }
        catch (Exception ex)
        {
            return new Result<UserLoginResponseModel>(ex);
        }
    }
}
