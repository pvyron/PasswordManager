using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Queries.Passwords;

public sealed record GetAllPasswordsQuery : IStreamRequest<PasswordResponseModel>;

public sealed class GetAllPasswordsQueryHandler : IStreamRequestHandler<GetAllPasswordsQuery, PasswordResponseModel>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;

    public GetAllPasswordsQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
    }

    public async IAsyncEnumerable<PasswordResponseModel> Handle(GetAllPasswordsQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            throw new AuthenticationException("You are not authorized for this action");
        }

        await foreach (var password in _passwordService.GetAllUserPasswords(Guid.Parse(userId), cancellationToken))
        {
            yield return new PasswordResponseModel
            {
                CategoryId = password.CategoryId,
                Description = password.Description,
                Id = password.Id,
                Password = password.Password,
                Username = password.Username,
                Title = password.Title,
            };
        }
    }
}