
namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public sealed record GetEmployeeAttendanceResponse(
        DateOnly Date,
        DateTime ClockIn,
        DateTime? ClockOut,
        string Status,
        int? TotalMinutes,
        int LateMinutes,
        int OvertimeMinutes
        );
}
