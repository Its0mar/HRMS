using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Employees.CreateEmployee;

using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ApiController
    {

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            CreateEmployeeCommand command,
            [FromServices] ICommandHandler<CreateEmployeeCommand, CreateEmployeeResponse> handler, 
            CancellationToken cancellationToken)
        {
           var result =  await handler.HandleAsync(
                command,
                cancellationToken);

            return result.Match<IActionResult>(
                response => Ok(result.Value),
                Problem);
        }
    }
}
