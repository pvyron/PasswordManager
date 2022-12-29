using MediatR;
using Microsoft.AspNetCore.Http;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Categories;
using PasswordManager.Application.IServices;
using PasswordManager.Application.Queries.Passwords;
using PasswordManager.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Queries.Categories;

public sealed record GetAllCategoriesQuery : IStreamRequest<CategoryResponseModel>;

public sealed class GetAllCategoriesQueryHandler : IStreamRequestHandler<GetAllCategoriesQuery, CategoryResponseModel>
{
    private readonly HttpContext _httpContext;
    private readonly IPasswordCategoriesService _passwordCategoriesService;

    public GetAllCategoriesQueryHandler(IHttpContextAccessor httpContextAccessor, IPasswordCategoriesService passwordCategoriesService)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _passwordCategoriesService = passwordCategoriesService;
    }

    public async IAsyncEnumerable<CategoryResponseModel> Handle(GetAllCategoriesQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

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