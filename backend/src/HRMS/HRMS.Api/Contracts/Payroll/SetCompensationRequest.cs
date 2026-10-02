using HRMS.Application.Features.Payroll.Compensations.Dtos;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Api.Contracts.Payroll
{
    public record SetCompensationRequest(
         int EmployeeId,
        decimal BasicSalary,
        string Currency,
        PayFrequency PayFrequency,
        DateTime EffectiveFrom,
        List<CompensationAllowanceDto> Allowances
        );
}
