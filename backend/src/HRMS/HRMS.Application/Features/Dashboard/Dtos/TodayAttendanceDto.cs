namespace HRMS.Application.Features.Dashboard.Dtos
{
    public record TodayAttendanceDto(
        int Id,
        DateOnly Date,
        string ClockIn,
        string? ClockOut,
        string Status,
        int? TotalMinutes,
        int LateMinutes
        );
}
