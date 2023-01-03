using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Categories;
using PasswordManager.Application.IServices;
using PasswordManager.Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace PasswordManager.Application.Queries.Categories;

public sealed record GetAllCategoriesQuery : IStreamRequest<CategoryResponseModel>;

public sealed class GetAllCategoriesQueryHandler : IStreamRequestHandler<GetAllCategoriesQuery, CategoryResponseModel>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordCategoriesService _passwordCategoriesService;

    public GetAllCategoriesQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordCategoriesService passwordCategoriesService)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordCategoriesService = passwordCategoriesService;
    }

    public async IAsyncEnumerable<CategoryResponseModel> Handle(GetAllCategoriesQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            throw new AuthenticationException("You are not authorized for this action");
        }

        await foreach (var category in _passwordCategoriesService.GetAllUserCategories(Guid.Parse(userId), cancellationToken))
        {
            yield return new CategoryResponseModel
            {
                Description = category.Description,
                Id = category.Id,
                Title = category.Title,
            };
        }
    }
}