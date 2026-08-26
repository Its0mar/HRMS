namespace HRMS.Application.Abstractions.Persistence.Models
{
    public record MyLeaveBalancesResponse(
        int LeaveTypeId,
        string LeaveTypeName,
        bool IsPaid,
        bool RequiresApproval,
        int Year,
        int TotalEntitledDays,
        decimal UsedDays,
        decimal PendingDays,
        decimal RemainingDays,
        bool IsRecordedInDb
        );
}