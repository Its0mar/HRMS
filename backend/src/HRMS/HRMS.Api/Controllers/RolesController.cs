using Asp.Versioning;
using HRMS.Api.Contracts.Roles;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Roles.CreateRole;
using HRMS.Application.Features.Roles.GetRoleDetails;
using HRMS.Application.Features.Roles.GetRoles;
using HRMS.Application.Features.Roles.GetRolesOptions;
using HRMS.Application.Features.Roles.Permissions.GetPermissionOptions;
using HRMS.Application.Features.Roles.UpdateRole;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public class RolesController : ApiController
    {
        private readonly ICurrentUser _currentUser;

        public RolesController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> GetAll(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetRolesQuery();
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpPost]
        [Authorize(Permissions.Roles.Create)]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoleRequest request,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand(request.Name, request.PermissionIds);
            var result = await dispatcher.SendAsync(command, cancellationToken);

            return result.Match<IActionResult>(
                result => Ok(result),
                Problem);
        }

        [HttpGet("options")]
        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> GetRolesOptions(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetRolesOptionsQuery(_currentUser.OrganizationId);
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpGet("permissions")]
        [Authorize(Permissions.SystemPermissions.View)]
        public async Task<IActionResult> GetPermissionOptions(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetPermissionOptionsQuery();
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdQuery(id);
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpPut("{id:int}")]
        [Authorize(Permissions.Roles.Update)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRoleRequest request,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoleCommand(
                id,
                request.Name,
                request.PermissionIds);

            var result = await dispatcher.SendAsync(
                command,
                cancellationToken);

            return result.Match<IActionResult>(
                _ => NoContent(),
                Problem);
        }
    }
}
