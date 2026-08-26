namespace HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes
{
    public record LeaveTypeResponse(
        int Id,
        string Name,
        string Code,
        int DefaultDaysPerYear,
        bool IsPaid,
        bool RequiresApproval);
}
