using LanguageExt.Common;
using Mediator;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;
using PasswordManager.Shared.RequestModels;
using PasswordManager.Shared.ResponseModels;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record CreatePasswordCommand(PasswordRequestModel PasswordRequestModel) : IRequest<Result<PasswordResponseModel>>;

public sealed class CreatePasswordCommandHandler : IRequestHandler<CreatePasswordCommand, Result<PasswordResponseModel>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;

    public CreatePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
    }

    public async ValueTask<Result<PasswordResponseModel>> Handle(CreatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            var passwordModel = new PasswordModel
            {
                CategoryId = request.PasswordRequestModel.CategoryId,
                Description = request.PasswordRequestModel.Description,
                Id = Guid.NewGuid(),
                Password = request.PasswordRequestModel.Password,
                Title = request.PasswordRequestModel.Title,
                UserId = userGuid,
                Username = request.PasswordRequestModel.Username,
                IsFavorite = request.PasswordRequestModel.IsFavorite,
                ImageId = request.PasswordRequestModel.ImageId,
                Logo = null
            };

            var createdPassword = await _passwordService.SaveNewPassword(passwordModel, cancellationToken);

            return new PasswordResponseModel
            {
                Id = createdPassword.Id,
                CategoryId = createdPassword.CategoryId,
                Description = createdPassword.Description,
                Password = createdPassword.Password,
                Title = createdPassword.Title,
                Username = createdPassword.Username,
                IsFavorite = createdPassword.IsFavorite,
                ImageId = createdPassword.ImageId,
                ImageTitle = createdPassword.Logo?.Title,
                PublicUrl = createdPassword.Logo?.FileUrl,
                ThumbnailUrl = createdPassword.Logo?.ThumbnailUrl,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
