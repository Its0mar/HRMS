using HRMS.Application.Features.Attendance.GetOrganizationAttendance;
using HRMS.Domain.Entities.Attendance;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Mappers.Attendance
{
    public static class OrganizationAttendanceMapper
    {
        public static GetOrganizationAttendanceResponse Map(SqlDataReader reader)
        {
            return new GetOrganizationAttendanceResponse(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    reader.GetString(reader.GetOrdinal("EmployeeName")),
                    reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                    reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                    reader.GetFieldValue<DateOnly>(reader.GetOrdinal("Date")),
                    DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal("ClockIn")), DateTimeKind.Utc).ToString("o"),
                    reader.IsDBNull(reader.GetOrdinal("ClockOut"))
                        ? null
                        : DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal("ClockOut")), DateTimeKind.Utc).ToString("o"),
                    ((AttendanceStatus)reader.GetInt32(reader.GetOrdinal("Status"))).ToString(),
                    reader.IsDBNull(reader.GetOrdinal("TotalMinutes")) ? null : reader.GetInt32(reader.GetOrdinal("TotalMinutes")),
                    reader.GetInt32(reader.GetOrdinal("LateMinutes")),
                    reader.GetInt32(reader.GetOrdinal("OverTimeMinutes")),
                    reader.IsDBNull(reader.GetOrdinal("HasPendingCorrection")) ? false
                        : reader.GetInt32(reader.GetOrdinal("HasPendingCorrection")) == 1
                    );
        }
    }
}
