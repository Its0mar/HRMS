using HRMS.Domain.Entities.Payroll;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IPayrollRepository
    {
        Task<bool> SetCompensationAsync(EmployeeCompensation compensation, CancellationToken cancellationToken);
        Task<EmployeeCompensation?> GetActiveCompensationAsync(int employeeId, int organizationId, CancellationToken cancellationToken);
    }
}
