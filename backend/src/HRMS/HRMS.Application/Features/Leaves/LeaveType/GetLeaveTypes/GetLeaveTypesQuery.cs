using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveType.GetLeaveTypes
{
    public sealed record GetLeaveTypesQuery(int OrganizationId)
        : IQuery<IReadOnlyList<LeaveTypeResponse>>;

}
