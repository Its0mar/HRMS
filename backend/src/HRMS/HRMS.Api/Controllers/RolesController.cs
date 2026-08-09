using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Permissions.GetPermissionOptions;
using HRMS.Application.Features.Roles.CreateRole;
using HRMS.Application.Features.Roles.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public class RolesController : ApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(
            [FromServices] IQueryHandler<GetRolesQuery, IReadOnlyList<GetRoleResponse>> handler,
            CancellationToken cancellationToken)
        {
            var query = new GetRolesQuery();
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            CreateRoleCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var result = await dispatcher.SendAsync(
                command,
                cancellationToken
                );

            return result.Match<IActionResult>(
                result =>Ok(result),
                Problem);
        }

        [HttpGet("permissions")]
        [Authorize]
        public async Task<IActionResult> GetPermissionOptions(
            [FromServices]
            IQueryHandler<GetPermissionOptionsQuery,IReadOnlyList<PermissionOptionResponse>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(
                new GetPermissionOptionsQuery(),
                cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }
    }
}
