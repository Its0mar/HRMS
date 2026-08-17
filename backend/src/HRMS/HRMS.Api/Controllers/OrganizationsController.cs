using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Organizations.Registration;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public sealed class OrganizationsController(ICommandDispatcher dispatcher) : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> RegisterOrganization(
            RegisterOrganizationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);
            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status201Created, response),
                Problem);
        }
    }
}
