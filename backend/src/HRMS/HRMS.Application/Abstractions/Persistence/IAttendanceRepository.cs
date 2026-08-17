
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IAttendanceRepository
    {
        Task<int> ClockInAsync(AttendanceLog log, CancellationToken cancellationToken);
        Task<bool> ClockOutAsync(int attendanceLogId, DateTime clockOut, int totalMinutes, int overTimeMinutes, CancellationToken cancellationToken);
        Task<AttendanceLog?> GetTodayLogAsync(int employeeId, DateOnly date, CancellationToken cancellationToken);
        Task<IReadOnlyList<AttendanceLog>> GetMyRecordsAsync(int employeeId, CancellationToken cancellationToken);
    }
}
