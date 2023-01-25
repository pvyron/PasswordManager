using LanguageExt.Common;
using Mediator;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using PasswordManager.Shared.ResponseModels;

namespace PasswordManager.Application.Queries.Passwords;

public sealed record GetPasswordQuery(Guid Id) : IRequest<Result<PasswordResponseModel>>;

public sealed class GetPasswordQueryHandler : IRequestHandler<GetPasswordQuery, Result<PasswordResponseModel>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;

    public GetPasswordQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
    }

    public async ValueTask<Result<PasswordResponseModel>> Handle(GetPasswordQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

            if (userId is null)
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            var userGuid = Guid.Parse(userId);

            var password = await _passwordService.GetPasswordById(request.Id, cancellationToken);

            if (!password.UserId.Equals(userGuid))
            {
                return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
            }

            return new PasswordResponseModel
            {
                Id = password.Id,
                CategoryId = password.CategoryId,
                Description = password.Description,
                Password = password.Password,
                Title = password.Title,
                Username = password.Username,
                IsFavorite = password.IsFavorite,
                ImageId = password.ImageId,
                ImageTitle = password.Logo?.Title,
                PublicUrl = password.Logo?.FileUrl
            };
        }
        catch (Exception ex)
        {
            return new Result<PasswordResponseModel>(ex);
        }
    }
}
