using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Domain.Models;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record UpdatePasswordCommand(PasswordRequestModel PasswordRequestModel) : IRequest<Result<PasswordResponseModel>>;

public sealed class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Result<PasswordResponseModel>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;
    private readonly IPasswordCategoriesService _passwordCategoriesService;

    public UpdatePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService, IPasswordCategoriesService passwordCategoriesService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
        _passwordCategoriesService = passwordCategoriesService;
    }

    public async Task<Result<PasswordResponseModel>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            if (request.PasswordRequestModel.Id is null)
            {
                return new Result<PasswordResponseModel>(new PasswordAccessException("No specified password to update"));
            }

            var passwordModel = await _passwordService.GetPasswordById(userGuid, (Guid)request.PasswordRequestModel.Id, cancellationToken);

            if (!passwordModel.UserId.Equals(userGuid))
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            if (request.PasswordRequestModel.CategoryId is not null)
            {
                //var category = await _passwordCategoriesService.GetCategoryById((Guid)request.PasswordRequestModel.CategoryId, cancellationToken);

                //if (!category.UserId.Equals(userGuid))
                //{
                //    return new Result<PasswordResponseModel>(new PasswordCategoryAccessException("You are not authorized for this action"));
                //}
            }

            passwordModel = new PasswordModel
            {
                UserId = Guid.Empty,
                Id = (Guid)request.PasswordRequestModel.Id,
                Title = request.PasswordRequestModel.Title,
                Username = request.PasswordRequestModel.Username,
                Password = request.PasswordRequestModel.Password,
                CategoryId = request.PasswordRequestModel.CategoryId,
                Description = request.PasswordRequestModel.Description,
            };

            passwordModel = await _passwordService.UpdatePassword(passwordModel, cancellationToken);

            return new PasswordResponseModel
            {
                Id = passwordModel.Id,
                Title = passwordModel.Title,
                Username = passwordModel.Username,
                Password = passwordModel.Password,
                CategoryId = passwordModel.CategoryId,
                Description = passwordModel.Description,
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
