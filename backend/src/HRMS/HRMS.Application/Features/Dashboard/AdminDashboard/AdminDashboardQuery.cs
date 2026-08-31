using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Dashboard.AdminDashboard
{
    public record AdminDashboardQuery(int OrganizationId) : IQuery<AdminDashboardResponse>;
}
