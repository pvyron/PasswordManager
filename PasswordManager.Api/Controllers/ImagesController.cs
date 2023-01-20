using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Commands.Images;
using PasswordManager.Application.Queries.Images;

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ImagesController : MediatorControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> UploadPasswordLogo(IFormFile formFile, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UploadPasswordLogoCommand(formFile), cancellationToken);

        return result.Match<IActionResult>(
            Succ => Ok(Succ),
            Fail => StatusCode(500));
    }

    [HttpGet("{guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadPasswordLogo(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPasswordLogoQuery(guid), cancellationToken);

        return result.Match<IActionResult>(
            Succ => File(Succ, System.Net.Mime.MediaTypeNames.Image.Jpeg),
            Fail => StatusCode(500));
    }
}
