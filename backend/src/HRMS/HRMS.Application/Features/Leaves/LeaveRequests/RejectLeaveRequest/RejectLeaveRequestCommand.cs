using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.RejectLeaveRequest
{
    public record RejectLeaveRequestCommand(int RequestId, string RejectionReason) : ICommand<bool>;
}
