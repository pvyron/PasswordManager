using LanguageExt.Common;
using Mediator;
using PasswordManager.Application.DtObjects.Authorization;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Models;

namespace PasswordManager.Application.Commands.Authorization;

public sealed record RegisterUserCommand(UserRegistrationRequestModel UserRegistrationModel) : IRequest<Result<UserRegistrationResponseModel>>;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserRegistrationResponseModel>>
{
    private readonly IUsersService _usersService;

    public RegisterUserCommandHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async ValueTask<Result<UserRegistrationResponseModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newUserModel = new UserModel
            {
                Id = Guid.Empty,
                Email = request.UserRegistrationModel.Email,
                FirstName = request.UserRegistrationModel.FirstName,
                LastName = request.UserRegistrationModel.LastName
            };

            var createdUser = await _usersService.CreateUser(newUserModel, request.UserRegistrationModel.Password, cancellationToken);

            var newUserResponse = new UserRegistrationResponseModel
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };

            return newUserResponse;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
}
