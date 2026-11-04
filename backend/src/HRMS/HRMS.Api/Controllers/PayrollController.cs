using Asp.Versioning;
using HRMS.Api.Contracts.Payroll;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Payroll.Compensations.Commands.SetEmployeeCompensation;
using HRMS.Application.Features.Payroll.Compensations.Queries.GetActiveCompensation;
using HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun;
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
                request.Allowances,
                request.Deductions
            );
            var result = await commandHandler.HandleAsync(command, cancellationToken);
            return result.Match(
                success => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpGet("compensations/active/{employeeId:int}")]
        public async Task<IActionResult> GetActiveCompensation(
            [FromRoute] int employeeId,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetActiveCompensationQuery(employeeId); 
            var result = await queryDispatcher.SendAsync(query, cancellationToken);
            return result.Match(
                success => Ok(success),
                errors => Problem(errors)
            );
        }

        [HttpPost("runs/calculate")]
        public async Task<IActionResult> CalculatePayrollRun(
            [FromBody] CalculatePayrollRunRequest request,
            [FromServices] ICommandHandler<CalculatePayrollRunCommand, int> commandHandler,
            CancellationToken cancellationToken)
        {
            var command = new CalculatePayrollRunCommand(
                request.PayFrequency,
                request.Year,
                request.Month,
                request.Week
            );

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            return result.Match(
                payrollRunId => Ok(new { PayrollRunId = payrollRunId }),
                errors => Problem(errors)
            );
        }

    }
}
