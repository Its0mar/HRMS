
namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public sealed record GetEmployeeAttendanceResponse(
        DateOnly Date,
        string ClockIn,
        string? ClockOut,
        string Status,
        int? TotalMinutes,
        int LateMinutes,
        int OvertimeMinutes
        );
}
