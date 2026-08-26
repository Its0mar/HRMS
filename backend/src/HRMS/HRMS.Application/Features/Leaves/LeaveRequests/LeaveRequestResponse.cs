using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Leaves.LeaveRequests
{
    public record LeaveRequestResponse(
        int Id,
        string TypeName,
        DateTime StartDate,
        DateTime EndDate,
        decimal TotalDays,
        string Reason,
        LeaveRequestStatus Status,
        string? rejectionReason
        );
}