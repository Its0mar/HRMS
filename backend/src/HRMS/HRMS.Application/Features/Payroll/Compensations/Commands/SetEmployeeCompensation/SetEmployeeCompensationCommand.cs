using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Payroll.Compensations.Dtos;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Application.Features.Payroll.Compensations.Commands.SetEmployeeCompensation
{
    public record SetEmployeeCompensationCommand(
        int EmployeeId,
        decimal BasicSalary,
        string Currency,
        PayFrequency PayFrequency,
        DateTime EffectiveFrom,
        List<CompensationAllowanceDto> Allowances
        ) : ICommand<bool>;
}