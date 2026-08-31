using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests
{
    public record class GetOrganizationLeaveRequestsQuery(int? Status)
        : IQuery<IReadOnlyList<GetOrganizationLeaveRequestsResponse>>;
}
