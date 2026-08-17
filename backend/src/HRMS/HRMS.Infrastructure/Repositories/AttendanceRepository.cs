
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;
using HRMS.Infrastructure.Persistence;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    public class AttendanceRepository(ISqlExecutor sqlExecutor) : IAttendanceRepository
    {
        public async Task<int> ClockInAsync(AttendanceLog log, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteWithScalarIntAsync(
                "dbo.Attendance_ClockIn",
                cancellationToken,
                Int("@EmployeeId", log.EmployeeId),
                Int("@WorkScheduleId", log.WorkScheduleId),
                Int("@OrganizationId", log.OrganizationId),
                Date("@Date", log.Date),
                DateTime2("@ClockIn", log.ClockIn),
                Int("@Status", (int)log.Status),
                Int("@LateMinutes", log.LateMinutes));
        }

        public Task<bool> ClockOutAsync(int attendanceLogId, DateTime clockOut, int totalMinutes, int overTimeMinutes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AttendanceLog>> GetMyRecordsAsync(int employeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceLog?> GetTodayLogAsync(int employeeId, DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
