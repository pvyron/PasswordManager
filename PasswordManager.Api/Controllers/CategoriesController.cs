using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.DtObjects.Categories;
using PasswordManager.Application.Queries.Categories;
using PasswordManager.Application.Queries.Passwords;

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : MediatorControllerBase
{
    [HttpGet]
    public IAsyncEnumerable<CategoryResponseModel> Get(CancellationToken cancellationToken)
    {
        return Mediator.CreateStream(new GetAllCategoriesQuery(), cancellationToken);
    }
}
