using HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun;
using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IPayrollRepository
    {
        Task<bool> SetCompensationAsync(EmployeeCompensation compensation, CancellationToken cancellationToken);
        Task<EmployeeCompensation?> GetActiveCompensationAsync(int employeeId, int organizationId, CancellationToken cancellationToken);
        public Task<bool> PayrollRunExistsAsync(
            int organizationId,
            int year,
            int? month,
            int? week,
            PayFrequency payFrequency,
            CancellationToken cancellationToken);
        public Task<int> CreatePayrollRunAsync(PayrollRun run, CancellationToken cancellationToken);
        Task<IReadOnlyList<EmployeePayrollCalculationDto>> GetEmployeesForCalculationAsync(
            int organizationId,
            PayFrequency payFrequency,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken);

    }
}
