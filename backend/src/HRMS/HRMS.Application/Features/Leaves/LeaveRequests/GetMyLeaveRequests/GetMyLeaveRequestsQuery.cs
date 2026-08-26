using HRMS.Application.Abstractions.Messaging;
using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests
{
    public record GetMyLeaveRequestsQuery(int EmployeeId, int OrganizationId) : IQuery<IReadOnlyList<LeaveRequestResponse>>;
}
