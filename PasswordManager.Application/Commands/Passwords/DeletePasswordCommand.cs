using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record DeletePasswordCommand(Guid PasswordGuid) : IRequest<Result<Unit>>;

public sealed class DeletePasswordCommandHandler : IRequestHandler<DeletePasswordCommand, Result<Unit>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;

    public DeletePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
    }

    public async Task<Result<Unit>> Handle(DeletePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<Unit>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            var passwordToDelete = await _passwordService.GetPasswordById(request.PasswordGuid, cancellationToken);

            if (!passwordToDelete.UserId.Equals(userGuid))
            {
                return new Result<Unit>(new AuthenticationException("You are not authorized for this action"));
            }

            await _passwordService.DeletePassword(request.PasswordGuid, cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }
}
