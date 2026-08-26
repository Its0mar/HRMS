namespace HRMS.Application.Abstractions.Persistence.Models
{
    public record MyLeaveBalancesResponse(
        int LeaveTypeId,
        string LeaveTypeName,
        bool IsPaid,
        bool RequiresApproval,
        int Year,
        int TotalEntitledDays,
        int UsedDays,
        int PendingDays,
        int RemainingDays
        );
}