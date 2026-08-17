using HRMS.Domain.Entities.Attendance;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class AttendanceLogMapper
    {
        public static AttendanceLog Map(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
            var workScheduleId = reader.GetInt32(reader.GetOrdinal("WorkScheduleId"));
            var organizationId = reader.GetInt32(reader.GetOrdinal("OrganizationId"));
            var date = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("Date"));
            var clockIn = reader.GetDateTime(reader.GetOrdinal("ClockIn"));
            var clockOutIndex = reader.GetOrdinal("ClockOut");
            DateTime? clockOut = reader.IsDBNull(clockOutIndex) ? null : reader.GetDateTime(clockOutIndex);
            var status = (AttendanceStatus)reader.GetInt32(reader.GetOrdinal("Status"));
            var totalMinutesIndex = reader.GetOrdinal("TotalMinutes");
            int? totalMinutes = reader.IsDBNull(totalMinutesIndex) ? null : reader.GetInt32(totalMinutesIndex);
            var lateMinutes = reader.GetInt32(reader.GetOrdinal("LateMinutes"));
            var overTimeMinutes = reader.GetInt32(reader.GetOrdinal("OverTimeMinutes"));
            var notesIndex = reader.GetOrdinal("Notes");
            string? notes = reader.IsDBNull(notesIndex) ? null : reader.GetString(notesIndex);

            return AttendanceLog.Restore(
                id, employeeId, workScheduleId, organizationId, date, clockIn, clockOut, status, totalMinutes, lateMinutes, overTimeMinutes, notes
                );
        }
    }
}
