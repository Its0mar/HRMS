using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.ApproveLeaveRequest
{
    public sealed record ApproveLeaveRequestCommand(
        int RequestId) : ICommand<bool>;
}
