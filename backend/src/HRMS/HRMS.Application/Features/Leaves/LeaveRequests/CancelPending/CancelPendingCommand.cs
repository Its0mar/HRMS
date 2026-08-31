using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.CancelPending
{
    public record class CancelPendingCommand(
        int Id,
        int EmployeeId,
        int OrganizationId) : ICommand<bool>;
}
