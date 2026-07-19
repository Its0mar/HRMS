using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Authentication.Login;
using HRMS.Application.Features.Authentication.RegisterOrganization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController : ApiController
{
    [HttpPost("organizations")]
    public async Task<IActionResult> RegisterOrganization(
        RegisterOrganizationCommand command,
        [FromServices] ICommandHandler<RegisterOrganizationCommand, RegisterOrganizationResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        return result.Match<IActionResult>(
            response => StatusCode(StatusCodes.Status201Created, response),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginCommand command,
        [FromServices]
        ICommandHandler<LoginCommand, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            command,
            cancellationToken);

        return result.Match<IActionResult>(
            Ok,
            Problem);
    }
}
