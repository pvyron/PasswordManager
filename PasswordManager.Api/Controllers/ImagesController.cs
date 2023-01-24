using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Commands.Images;
using PasswordManager.Application.Queries.Images;
using PasswordManager.Shared.ResponseModels;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ImagesController : MediatorControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadPasswordLogo([FromHeader] string imageTitle, IFormFile formFile, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UploadPasswordLogoCommand(formFile, imageTitle), cancellationToken);

        return result.Match<IActionResult>(
            Succ =>
            {
                if (string.IsNullOrWhiteSpace(Succ.PublicUrl))
                    return CreatedAtAction(nameof(UploadPasswordLogo), Succ);

                return Created(Succ.PublicUrl ?? "", Succ);
            },
            Fail => StatusCode(500));
    }

    [HttpGet]
    public async IAsyncEnumerable<ImageLogoResponseModel> GetPasswordLogos([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var responseModel in Mediator.CreateStream(new GetPasswordLogosQuery(), cancellationToken))
        {
            yield return responseModel;
        }
    }

    [HttpGet("{guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadPasswordLogo(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPasswordLogoQuery(guid), cancellationToken);

        return result.Match<IActionResult>(
            Succ => File(Succ, MediaTypeNames.Image.Jpeg),
            Fail => StatusCode(500));
    }
}
