using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Commands.Authorization;
using PasswordManager.Application.DtObjects.Authorization;
using PasswordManager.Application.Queries.Authorization;
using PasswordManager.Domain.Exceptions;
using System.Net.Mime;

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthorizationController : MediatorControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UserRegistrationResponseModel), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(UserRegistrationRequestModel requestModel, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new RegisterUserCommand(requestModel), cancellationToken);
        return response.Match<IActionResult>(
            Succ => CreatedAtAction(nameof(Register), Succ),
            ex =>
            {
                if (ex is AccessException<object>)
                {
                    return Unauthorized(ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserLoginResponseModel), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromHeader] string? email, [FromHeader] string? password, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new LoginUserQuery(email, password), cancellationToken);

        return response.Match<IActionResult>(
            Succ => CreatedAtAction(nameof(Register), Succ),
            ex =>
            {
                if (ex is AuthenticationException)
                {
                    return BadRequest(ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }
}
