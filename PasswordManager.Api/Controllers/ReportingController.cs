using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Queries.Reporting;

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ReportingController : MediatorControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPublicPasswordData(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new PublicPasswordDataQuery(), cancellationToken);

        return response.Match<IActionResult>(
            Ok,
            Fail =>
            {
                return StatusCode(500);
            });
    }
}
