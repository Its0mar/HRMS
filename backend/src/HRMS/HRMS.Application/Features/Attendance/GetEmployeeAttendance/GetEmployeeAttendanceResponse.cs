
namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public sealed record GetEmployeeAttendanceResponse(
        int Id,
        DateOnly Date,
        string ClockIn,
        string? ClockOut,
        string Status,
        int? TotalMinutes,
        int LateMinutes,
        int OvertimeMinutes
        );
}
