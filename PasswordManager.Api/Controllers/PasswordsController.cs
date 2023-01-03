using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Application.Commands;
using PasswordManager.Application.Commands.Passwords;
using PasswordManager.Application.DtObjects;
using PasswordManager.Application.DtObjects.Passwords;
using PasswordManager.Application.Queries.Passwords;
using PasswordManager.Domain.Exceptions;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PasswordsController : MediatorControllerBase
{
    [HttpGet]
    public async IAsyncEnumerable<PasswordResponseModel> Get([EnumeratorCancellation]CancellationToken cancellationToken)
    {
        await foreach (var password in Mediator.CreateStream(new GetAllPasswordsQuery(), cancellationToken))
        {
            yield return password;
        }
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
                    return Unauthorized(new ErrorResponse(Fail.Message, Fail));
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(new ErrorResponse(Fail.Message, Fail));
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
                    return Unauthorized(new ErrorResponse(Fail.Message, Fail));
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(new ErrorResponse(Fail.Message, Fail));
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] PasswordRequestModel passwordRequestModel, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new UpdatePasswordCommand(passwordRequestModel), cancellationToken);

        return response.Match<IActionResult>(
            Ok,
            Fail =>
            {
                if (Fail is AuthenticationException)
                {
                    return Unauthorized(new ErrorResponse(Fail.Message, Fail));
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(new ErrorResponse(Fail.Message, Fail));
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpDelete("{categoryId}/{id}")]
    public async Task<IActionResult> Delete(Guid categoryId, Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new DeletePasswordCommand(categoryId, id), cancellationToken);

        return response.Match<IActionResult>(
            _ => Ok(),
            Fail =>
            {
                if (Fail is AuthenticationException)
                {
                    return Unauthorized(new ErrorResponse(Fail.Message, Fail));
                }

                if (Fail is AccessException<object>)
                {
                    return BadRequest(new ErrorResponse(Fail.Message, Fail));
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            });
    }

    [HttpPatch]
    [AllowAnonymous]
    public async Task<IActionResult> Patch(CancellationToken cancellationToken)
    {
        await Mediator.Send(new PopulateDbWithRandomDataCommand(), cancellationToken);


        return Ok();
    }
}
