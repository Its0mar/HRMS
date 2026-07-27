using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Employees.CreateEmployee;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class EmployeesController : ApiController
    {

        [HttpPost]
        [Authorize(Policy = Permissions.Employees.Create)]
        public async Task<IActionResult> CreateAsync(
            CreateEmployeeCommand command,
            [FromServices] ICommandDispatcher dispatcher, 
            CancellationToken cancellationToken)
        {
           var result =  await dispatcher.SendAsync(
                command,
                cancellationToken);

            return result.Match<IActionResult>(
                response => Ok(result.Value),
                Problem);
        }
    }
}
