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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{

    [ApiController]
    [ApiVersion(1)]
    public class RolesController : ApiController
    {
        private ICurrentUser _currentUser;

        public RolesController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

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

        [HttpGet("options")]
        [Authorize]
        public async Task<IActionResult> GetRolesOptions(
            [FromServices] IQueryHandler<GetRolesOptionsQuery, IReadOnlyList<GetRolesOptionsResponse>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(
                new GetRolesOptionsQuery(_currentUser.OrganizationId),
                cancellationToken);

            return result.Match<IActionResult>(
                Ok,
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


        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(
            int id,
            [FromServices] IQueryHandler<GetRoleByIdQuery, GetRoleDetailsResponse> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(
                new GetRoleByIdQuery(id),
                cancellationToken);

            return result.Match<IActionResult>(
                Ok,
                Problem);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(
            int id,
            UpdateRoleRequest request,
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
