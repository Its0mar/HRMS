using HRMS.Application.Features.Payroll.Compensations.Dtos;

namespace HRMS.Application.Features.Payroll.Compensations.Queries.GetActiveCompensation
{
    public record GetActiveCompensationResponse(
        int Id,
        int EmployeeId,
        decimal BasicSalary,
        string Currency,
        string PayFrequency,
        DateTime EffectiveFrom,
        DateTime CreatedAt,
        List<CompensationAllowanceDto> Allowances,
        List<CompensationDeductionDto> Deductions,
        decimal TotalCompensation
        );
}