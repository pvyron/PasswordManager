using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.Api;

public abstract class MediatorControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}
