using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Dashboard.AdminDashboard
{
    public sealed class GetAdminDashboardHandler(IDashboardRepository dashboardRepository)
        : IQueryHandler<AdminDashboardQuery, AdminDashboardResponse>
    {
        public async Task<ErrorOr<AdminDashboardResponse>> HandleAsync(AdminDashboardQuery query, CancellationToken cancellationToken)
        {
            var summary = await dashboardRepository.GetForAdminAsync(query.OrganizationId, cancellationToken);
            if (summary is null)
            {
                return Error.NotFound("Dashboard.NotFound", "Could not load admin dashboard summary.");
            }

            return summary;
        }
    }
}
