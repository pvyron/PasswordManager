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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record CreatePasswordCommand (PasswordRequestModel PasswordRequestModel) : IRequest<Result<PasswordResponseModel>>;

public sealed class CreatePasswordCommandHandler : IRequestHandler<CreatePasswordCommand, Result<PasswordResponseModel>>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordService _passwordService;
    private readonly IUsersService _usersService;
    private readonly IPasswordCategoriesService _passwordCategoriesService;

    public CreatePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService, IUsersService usersService, IPasswordCategoriesService passwordCategoriesService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
        _usersService = usersService;
        _passwordCategoriesService = passwordCategoriesService;
    }

    public async Task<Result<PasswordResponseModel>> Handle(CreatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            _ = await _usersService.GetUserById(userGuid, cancellationToken);

            if (request.PasswordRequestModel.CategoryId is not null)
            {
                var category = await _passwordCategoriesService.GetCategoryById((Guid)request.PasswordRequestModel.CategoryId, cancellationToken);

                if (!category.UserId.Equals(userGuid))
                {
                    return new Result<PasswordResponseModel>(new PasswordCategoryAccessException("You are not authorized for this action"));
                }
            }

            var passwordModel = new PasswordModel
            {
                Id = Guid.Empty,
                UserId = userGuid,
                Title = request.PasswordRequestModel.Title,
                Username = request.PasswordRequestModel.Username,
                Password = request.PasswordRequestModel.Password,
                CategoryId = request.PasswordRequestModel.CategoryId,
                Description = request.PasswordRequestModel.Description,
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
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
