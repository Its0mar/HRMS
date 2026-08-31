using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Dashboard.AdminDashboard;
using HRMS.Application.Features.Dashboard.EmployeeDasboard;
using HRMS.Infrastructure.Mappers.Dashboard;
using HRMS.Infrastructure.Persistence;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    internal class DashboardRepository(ISqlExecutor sqlExecutor) : IDashboardRepository
    {
        public async Task<EmployeeDashboardResponse?> GetForEmployeeAsync(int employeeId, int organizationId, int year, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryMultipleAsync(
                "dbo.Dashboard_GetEmployeeSummary",
                EmployeeDashboardResponseMapper.MapAsync,
                cancellationToken,
                Int("@EmployeeId", employeeId),
                Int("@OrganizationId", organizationId),
                Int("@Year", year)
            );
        }

        public async Task<AdminDashboardResponse?> GetForAdminAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryMultipleAsync(
                "dbo.Dashboard_GetAdminSummary",
                AdminDashboardResponseMapper.MapAsync,
                cancellationToken,
                Int("@OrganizationId", organizationId)
            );
        }
    }
}
