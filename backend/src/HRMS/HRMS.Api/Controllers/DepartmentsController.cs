using Asp.Versioning;
using HRMS.Api.Contracts.Departments;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Departments.CreateDepartment;
using HRMS.Application.Features.Departments.GetDepartments;
using HRMS.Application.Features.Departments.UpdateDepartment;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{   
    [ApiController]
    [ApiVersion(1)]
    public class DepartmentsController(ICurrentUser currentUser) : ApiController
    {
        [Authorize(Policy = Permissions.Departments.Create)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateDepartmentRequest request,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken ct)
        {
            var command = new CreateDepartmentCommand(
                request.Name,
                request.Code,
                request.Description,
                request.ManagerId,
                currentUser.OrganizationId);

            var result = await dispatcher.SendAsync(command, ct);

            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status201Created, result.Value),
                Problem
             );
        }

        [Authorize(Policy = Permissions.Departments.Update)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(
            [FromBody] UpdateDepartmentRequest request,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken ct)
        {
            var command = new UpdateDepartmentCommand(
                request.Id,
                request.Name,
                request.Description,
                request.ManagerEmployeeId,
                currentUser.OrganizationId);

            var result = await dispatcher.SendAsync(command, ct);

            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status200OK),
                Problem
             );
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Departments.View)]
        public async Task<IActionResult> GetAsync(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetDepartmentsQuery(currentUser.OrganizationId);
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                ok => Ok(result.Value),
                Problem);
        }
    }
}
