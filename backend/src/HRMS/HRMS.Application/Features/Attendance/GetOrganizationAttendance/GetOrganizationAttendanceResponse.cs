namespace HRMS.Application.Features.Attendance.GetOrganizationAttendance
{
    public sealed record GetOrganizationAttendanceResponse(
    int Id,
    int EmployeeId,
    string EmployeeName,
    string EmployeeCode,
    string? DepartmentName,
    DateOnly Date,
    string ClockIn,
    string? ClockOut,
    string Status,
    int? TotalMinutes,
    int LateMinutes,
    int OvertimeMinutes,
    bool PendingCorrectionStatus);
}
