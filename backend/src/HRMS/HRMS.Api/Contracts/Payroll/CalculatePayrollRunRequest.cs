using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Api.Contracts.Payroll
{
    public record CalculatePayrollRunRequest(
        PayFrequency PayFrequency,
        int Year,
        int? Month = null,
        int? Week = null
    );
}
