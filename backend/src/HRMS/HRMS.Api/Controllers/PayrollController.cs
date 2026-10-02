using Asp.Versioning;
using HRMS.Api.Contracts.Payroll;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Payroll.Compensations.Commands.SetEmployeeCompensation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiVersion(1)]
    [Authorize]
    public class PayrollController : ApiController
    {
        [HttpPost("compensations")]
        public async Task<IActionResult> SetEmployeeCompensation(
            [FromBody] SetCompensationRequest request,
            [FromServices] ICommandHandler<SetEmployeeCompensationCommand, bool> commandHandler,
            CancellationToken cancellationToken)
        {
            var command = new SetEmployeeCompensationCommand(
                request.EmployeeId,
                request.BasicSalary,
                request.Currency,
                request.PayFrequency,
                request.EffectiveFrom,
                request.Allowances
            );
            var result = await commandHandler.HandleAsync(command, cancellationToken);
            return result.Match(
                success => Ok(),
                errors => Problem(errors)
            );
        }

    }
}
