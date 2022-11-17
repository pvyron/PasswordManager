using MediatR;
using PasswordManager.Api.Models.RequestModels;
using PasswordManager.Api.Models.ResponseModels;

namespace PasswordManager.Api.Commands.Users
{
    public class CreateUserCommand : IRequest<UserResponseModel>
    {
        public UserRequestModel UserRequestModel { get; private set; }

        public CreateUserCommand(UserRequestModel userRequestModel)
        {
            UserRequestModel = userRequestModel;
        }
    }
}
