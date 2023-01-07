using LanguageExt.Common;
using MediatR;
using PasswordManager.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Commands.Authorization;

public sealed record ResetAllUserPasswordsCommand(string NewPassword) : IRequest<Result<Unit>>;

public sealed class ResetAllUserPasswordsCommandHandler : IRequestHandler<ResetAllUserPasswordsCommand, Result<Unit>>
{
    private readonly IUsersService _usersService;

    public ResetAllUserPasswordsCommandHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<Result<Unit>> Handle(ResetAllUserPasswordsCommand request, CancellationToken cancellationToken)
    {
        await _usersService.ResetAllUsersPassword(request.NewPassword, cancellationToken);

        return Unit.Value;
    }
}
