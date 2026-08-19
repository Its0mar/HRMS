using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
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

        public async Task<bool> ClockOutAsync(int attendanceLogId, DateTime clockOut, int totalMinutes, int overTimeMinutes, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.Attendance_ClockOut",
                cancellationToken,
                Int("@AttendanceLogId", attendanceLogId),
                DateTime2("@ClockOut", clockOut),
                Int("@TotalMinutes", totalMinutes),
                Int("@OverTimeMinutes", overTimeMinutes));
        }

        public async Task<IReadOnlyList<AttendanceLog>> GetMyRecordsAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "dbo.Attendance_GetMyRecord",
                AttendanceLogMapper.Map,
                cancellationToken,
                new SqlParameter("@EmployeeId", employeeId));

        }

        public async Task<AttendanceLog?> GetTodayLogForEmployeeAsync(int employeeId, DateOnly date, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.Attendance_GetTodayStatus",
                AttendanceLogMapper.Map,
                cancellationToken,
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@Date", date));
        }

        public async Task<int> CreateAttendanceCorrectionAsync(AttendanceCorrection attendanceCorrection, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteWithScalarIntAsync(
                "dbo.Attendance_CreateAttendanceCorrection",
                cancellationToken,
                Int("@OrganizationId", attendanceCorrection.OrganizationId),
                Int("@EmployeeId", attendanceCorrection.EmployeeId),
                NullableInt("@AttendanceLogId", attendanceCorrection.AttendanceLogId),
                DateTime2("@RequestedClockIn", attendanceCorrection.RequestedClockIn),
                DateTime2("@RequestedClockOut", attendanceCorrection.RequestedClockOut),
                VarChar("@Reason", 300, attendanceCorrection.Reason)
                );
        }
    }
}