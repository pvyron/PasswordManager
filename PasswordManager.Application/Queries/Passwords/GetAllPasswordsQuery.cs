using LanguageExt.Common;
using MediatR;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Application.Queries.Passwords;

public sealed record GetAllPasswordsQuery : IStreamRequest<PasswordResponseModel>;

public sealed class GetAllPasswordsQueryHandler : IStreamRequestHandler<GetAllPasswordsQuery, PasswordResponseModel>
{
    private readonly IPasswordCategoriesService _passwordCategoriesService;

    public GetAllPasswordsQueryHandler(IPasswordCategoriesService passwordCategoriesService)
    {
        _passwordCategoriesService = passwordCategoriesService;
    }

    public async IAsyncEnumerable<PasswordResponseModel> Handle(GetAllPasswordsQuery request,[EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userId = _httpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        if (userId is null)
        {
            return new Result<PasswordResponseModel>(new AuthenticationException("You are not authorized for this action"));
        }
    }
}