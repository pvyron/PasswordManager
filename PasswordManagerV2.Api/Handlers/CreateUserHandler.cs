using MediatR;
using PasswordManager.Api.Commands.Users;
using PasswordManager.Api.Models.ResponseModels;

namespace PasswordManager.Api.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponseModel?>
    {
        public async Task<UserResponseModel?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await Handle(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
