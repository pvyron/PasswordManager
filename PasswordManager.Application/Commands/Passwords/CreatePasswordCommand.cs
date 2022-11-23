﻿using LanguageExt.Common;
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

    public CreatePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordService = passwordService;
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

            var passwordModel = new PasswordModel
            {
                Id = Guid.Empty,
                UserId = Guid.Parse(userId),
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
