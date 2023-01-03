using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.DtObjects.Categories;
using PasswordManager.Application.Queries.Categories;

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
