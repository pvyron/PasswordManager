using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Shared.ResponseModels;

namespace PasswordManager.Application.Commands.Passwords;

public sealed record AlternateFavorabilityCommand(Guid Id, bool Favorite) : IRequest<Result<PasswordResponseModel>>;

public sealed class AlternateFavorabilityCommandHandler : IRequestHandler<AlternateFavorabilityCommand, Result<PasswordResponseModel>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;

    public AlternateFavorabilityCommandHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
    }

    public async Task<Result<PasswordResponseModel>> Handle(AlternateFavorabilityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            var passwordModel = await _passwordService.GetPasswordById(request.Id, cancellationToken);

            if (!passwordModel.UserId.Equals(userGuid))
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var updatedPassword = await _passwordService.FavoritePassword(request.Id, request.Favorite, cancellationToken);

            return new PasswordResponseModel
            {
                Username = updatedPassword.Username,
                CategoryId = updatedPassword.CategoryId,
                Description = updatedPassword.Description,
                Id = updatedPassword.Id,
                IsFavorite = updatedPassword.IsFavorite,
                Password = updatedPassword.Password,
                Title = updatedPassword.Title,
                ImageId = updatedPassword.ImageId,
                ImageTitle = updatedPassword.Logo?.Title,
                PublicUrl = updatedPassword.Logo?.FileUrl,
                ThumbnailUrl = updatedPassword.Logo?.ThumbnailUrl
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
