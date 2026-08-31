using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests
{
    public record GetMyLeaveRequestResponse(
        int Id,
        string TypeName,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal TotalDays,
        string Reason,
        LeaveRequestStatus Status,
        string? rejectionReason
        );
}