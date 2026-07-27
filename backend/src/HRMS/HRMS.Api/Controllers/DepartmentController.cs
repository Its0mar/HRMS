using Asp.Versioning;
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
    public class DepartmentController : ApiController
    {
        [Authorize(Policy = Permissions.Departments.Create)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(
            CreateDepartmentCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken ct)
        {
            var result = await dispatcher.SendAsync(command, ct);

            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status201Created, result.Value),
                Problem
             );
        }

        [Authorize(Policy = Permissions.Departments.Update)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(
            UpdateDepartmentCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken ct)
        {
            var result = await dispatcher.SendAsync(command, ct);

            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status200OK),
                Problem
                );
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Departments.View)]
        public async Task<IActionResult> GetAsync(
            GetDepartmentsQuery query,
            [FromServices] IQueryHandler<GetDepartmentsQuery, List<GetDepartmentResponse>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.Match(
                ok => Ok(result.Value),
                Problem);
        }
    }
}
