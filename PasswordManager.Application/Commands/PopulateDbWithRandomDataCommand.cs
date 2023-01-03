using LanguageExt.Common;
using MediatR;
using PasswordManager.Application.IServices;

namespace PasswordManager.Application.Commands;

public sealed record PopulateDbWithRandomDataCommand : IRequest<Result<Unit>>;

public sealed class PopulateDbWithRandomDataCommandHandler : IRequestHandler<PopulateDbWithRandomDataCommand, Result<Unit>>
{
    private readonly IUsersService _usersService;

    public PopulateDbWithRandomDataCommandHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<Result<Unit>> Handle(PopulateDbWithRandomDataCommand request, CancellationToken cancellationToken)
    {
        await _usersService.PopulateDb(cancellationToken);

        return Unit.Value;
    }
}
