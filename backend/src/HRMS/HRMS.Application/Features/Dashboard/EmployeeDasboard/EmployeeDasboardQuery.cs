using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Dashboard.EmployeeDasboard
{
    public record EmployeeDasboardQuery(
        int EmployeeId,
        int OrganizationId,
        int Year
        ) : IQuery<EmployeeDashboardResponse>;
}
