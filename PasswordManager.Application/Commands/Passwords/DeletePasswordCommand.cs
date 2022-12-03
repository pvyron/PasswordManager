using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record DeletePasswordCommand(Guid CategoryGuid, Guid PasswordGuid) : IRequest<Result<Unit>>;

public sealed class DeletePasswordCommandHandler : IRequestHandler<DeletePasswordCommand, Result<Unit>>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;

    public DeletePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
    }

    public async Task<Result<Unit>> Handle(DeletePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<Unit>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            await _passwordService.DeletePassword(userGuid, request.CategoryGuid, request.PasswordGuid, cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }
}
