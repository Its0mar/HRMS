using HRMS.Application.Abstractions.Messaging;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun
{
    public record CalculatePayrollRunCommand(
       PayFrequency PayFrequency,
       int Year,
       int? Month = null,
       int? Week = null
   ) : ICommand<int>;
}
