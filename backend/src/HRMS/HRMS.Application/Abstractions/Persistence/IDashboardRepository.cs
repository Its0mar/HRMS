using HRMS.Application.Features.Dashboard.AdminDashboard;
using HRMS.Application.Features.Dashboard.EmployeeDasboard;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IDashboardRepository
    {
        Task<EmployeeDashboardResponse?> GetForEmployeeAsync(int employeeId, int organizationId, int year, CancellationToken cancellationToken);
        Task<AdminDashboardResponse?> GetForAdminAsync(int organizationId, CancellationToken cancellationToken);
    }
}
