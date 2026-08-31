using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Dashboard.EmployeeDasboard
{
    internal class EmployeeDasboardHandler(IDashboardRepository dashboardRepository)
        : IQueryHandler<EmployeeDasboardQuery, EmployeeDashboardResponse>
    {
        public async Task<ErrorOr<EmployeeDashboardResponse>> HandleAsync(EmployeeDasboardQuery query, CancellationToken cancellationToken)
        {
            var dasboardData =  await dashboardRepository.GetForEmployeeAsync(query.EmployeeId, query.OrganizationId, query.Year, cancellationToken);

            if (dasboardData is null) return Error.Failure(description: "could not get the dashboard for the employee");

            return dasboardData;
        }
    }
}
