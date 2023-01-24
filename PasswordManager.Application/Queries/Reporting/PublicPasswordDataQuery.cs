using LanguageExt.Common;
using Mediator;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Reporting;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;

namespace PasswordManager.Application.Queries.Reporting;

public sealed record PublicPasswordDataQuery : IRequest<Result<IEnumerable<PublicPasswordDataResponseModel>>>;

public sealed class PublicPasswordDataQueryHandler : IRequestHandler<PublicPasswordDataQuery, Result<IEnumerable<PublicPasswordDataResponseModel>>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordCategoriesService _passwordCategoriesService;
    private readonly IPasswordService _passwordService;

    public PublicPasswordDataQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordCategoriesService passwordCategoriesService, IPasswordService passwordService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordCategoriesService = passwordCategoriesService;
        _passwordService = passwordService;
    }

    public async ValueTask<Result<IEnumerable<PublicPasswordDataResponseModel>>> Handle(PublicPasswordDataQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId);

            if (userId is null)
            {
                return new Result<IEnumerable<PublicPasswordDataResponseModel>>(new AuthenticationException("Unauthorized"));
            }

            var userGuid = Guid.Parse(userId.Value);

            var categories = (await _passwordCategoriesService.GetAllUserCategories(userGuid, cancellationToken).ToListAsync(cancellationToken)).ToDictionary(c => c.Id, c => c);

            var passwords = new List<PublicPasswordDataResponseModel>();

            await foreach (var password in _passwordService.GetAllUserPasswords(userGuid, cancellationToken))
            {
                passwords.Add(new PublicPasswordDataResponseModel
                {
                    CategoryName = categories[password.CategoryId!.Value].Title,
                    Description = password.Description,
                    IsFavorite = password.IsFavorite.GetValueOrDefault(false),
                    PasswordId = password.Id,
                    PasswordTitle = password.Title,
                    Username = password.Username,
                });
            }

            return passwords;
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<PublicPasswordDataResponseModel>>(ex);
        }
    }
}