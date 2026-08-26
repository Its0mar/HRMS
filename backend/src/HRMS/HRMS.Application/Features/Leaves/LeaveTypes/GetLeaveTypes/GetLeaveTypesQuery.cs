using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes
{
    public sealed record GetLeaveTypesQuery(int OrganizationId)
        : IQuery<IReadOnlyList<LeaveTypeResponse>>;

}
