
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Departments.CreateDepartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class DepartmentController : ApiController
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(
            CreateDepartmentCommand command,
            [FromServices] ICommandHandler<CreateDepartmentCommand, CreateDepartmentResponse> handler,
            CancellationToken ct)
        {
            var result = await handler.HandleAsync(command, ct);

            return result.Match<IActionResult>(
                response => StatusCode(StatusCodes.Status201Created, result),
                Problem
             );
        }
    }
}
