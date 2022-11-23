using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Commands.Passwords;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.Queries.Passwords;
using PasswordManager.Domain.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PasswordsController : MediatorControllerBase
{
    [HttpGet]
    public IAsyncEnumerable<PasswordResponseModel> Get(CancellationToken cancellationToken)
    {
        return Mediator.CreateStream(new GetAllPasswordsQuery(), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetPasswordQuery(id), cancellationToken);

        return response.Match<IActionResult>(
            Ok,
            Fail =>
            {
                if (Fail is AuthenticationException)
                {
                    return Unauthorized(Fail.Message);
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(Fail.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PasswordRequestModel passwordRequestModel, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreatePasswordCommand(passwordRequestModel), cancellationToken);

        return response.Match<IActionResult>(
            Ok,
            Fail =>
            {
                if (Fail is AuthenticationException)
                {
                    return Unauthorized(Fail.Message);
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(Fail.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromBody] PasswordRequestModel passwordRequestModel, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new UpdatePasswordCommand(passwordRequestModel), cancellationToken);

        return response.Match<IActionResult>(
            Ok,
            Fail =>
            {
                if (Fail is AuthenticationException)
                {
                    return Unauthorized(Fail.Message);
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(Fail.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok();
    }
}
