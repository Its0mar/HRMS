using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveType.UpdateLeaveType
{
    public record UpdateLeaveTypeCommand(
        int Id,
        string Name,
        string Code,
        int DefaultDaysPerYear,
        bool IsPaid,
        bool RequiresApproval) : ICommand<bool>;
}
