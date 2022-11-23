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

namespace PasswordManager.Application.Commands.Passwords;

public sealed record UpdatePasswordCommand(PasswordRequestModel PasswordRequestModel) : IRequest<Result<PasswordResponseModel>>;

public sealed class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Result<PasswordResponseModel>>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;

    public UpdatePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
    }

    public async Task<Result<PasswordResponseModel>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
        }

        throw;
    }
}
