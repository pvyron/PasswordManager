using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Queries.Passwords;

public sealed record GetPasswordQuery(Guid Id) : IRequest<Result<PasswordResponseModel>>;

public sealed class GetPasswordQueryHandler : IRequestHandler<GetPasswordQuery, Result<PasswordResponseModel>>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;

    public GetPasswordQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
    }

    public async Task<Result<PasswordResponseModel>> Handle(GetPasswordQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            var password = await _passwordService.GetPasswordById(userGuid, request.Id, cancellationToken);

            if (!password.UserId.Equals(userGuid))
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            return new PasswordResponseModel
            {
                Id = password.Id,
                CategoryId = password.CategoryId,
                Description = password.Description,
                Password = password.Password,
                Title = password.Title,
                Username = password.Username,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
