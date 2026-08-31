using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests
{
    public record GetMyLeaveRequestsQuery(int EmployeeId, int OrganizationId) : IQuery<IReadOnlyList<GetMyLeaveRequestResponse>>;
}
