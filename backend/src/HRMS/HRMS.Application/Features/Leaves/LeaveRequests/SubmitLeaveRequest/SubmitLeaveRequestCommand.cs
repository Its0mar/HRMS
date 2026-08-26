using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.SubmitLeaveRequest
{
    public sealed record SubmitLeaveRequestCommand(
        int LeaveTypeId,
        DateTime StartDate,
        DateTime EndDate,
        string Reason
        ) : ICommand<bool>;
}
