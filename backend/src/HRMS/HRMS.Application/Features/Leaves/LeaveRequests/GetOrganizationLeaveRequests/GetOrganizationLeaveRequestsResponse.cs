using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests
{
    public sealed record GetOrganizationLeaveRequestsResponse(
        int Id,
        int EmployeeId,
        string EmployeeName,
        string EmployeeNumber,
        string LeaveName,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal TotalDays,
        string Reason,
        LeaveRequestStatus Status,
        string? ReviewdByEmployee,
        DateTime? ReviewdAt,
        string? RejectionReason
        );
}
