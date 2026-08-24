namespace HRMS.Application.Features.Leaves.LeaveType.GetLeaveTypes
{
    public record LeaveTypeResponse(
        int Id,
        string Name,
        string Code,
        int DefaultDaysPerYear,
        bool IsPaid,
        bool RequiresApproval);
}
