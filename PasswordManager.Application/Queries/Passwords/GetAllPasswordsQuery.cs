using Mediator;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.IServices;
using PasswordManager.Shared.ResponseModels;
using System.Runtime.CompilerServices;
using System.Security.Authentication;

namespace PasswordManager.Application.Queries.Passwords;

public sealed record GetAllPasswordsQuery : IStreamRequest<PasswordResponseModel>;

public sealed class GetAllPasswordsQueryHandler : IStreamRequestHandler<GetAllPasswordsQuery, PasswordResponseModel>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordService _passwordService;

    public GetAllPasswordsQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordService = passwordService;
    }

    public async IAsyncEnumerable<PasswordResponseModel> Handle(GetAllPasswordsQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            throw new AuthenticationException("You are not authorized for this action");
        }

        await foreach (var password in _passwordService.GetAllUserPasswords(Guid.Parse(userId), cancellationToken))
        {
            yield return new PasswordResponseModel
            {
                CategoryId = password.CategoryId,
                Description = password.Description,
                Id = password.Id,
                Password = password.Password,
                Username = password.Username,
                Title = password.Title,
                IsFavorite = password.IsFavorite,
                ImageId = password.ImageId,
                ImageTitle = password.Logo?.Title,
                PublicUrl = password.Logo?.FileUrl,
                ThumbnailUrl = password.Logo?.ThumbnailUrl
            };
        }
    }
}