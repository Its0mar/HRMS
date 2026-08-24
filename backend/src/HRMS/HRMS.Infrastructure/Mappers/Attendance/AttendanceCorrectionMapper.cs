using HRMS.Domain.Entities.Attendance;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Attendance
{
    public static class AttendanceCorrectionMapper
    {
        public static AttendanceCorrection Map(SqlDataReader reader)
        {

            var attendanceLogIdIndex = reader.GetOrdinal("AttendanceLogId");
            var attendanceLogId = reader.IsDBNull(attendanceLogIdIndex) ? (int?) null : reader.GetInt32(attendanceLogIdIndex);


            var reviewedByIdindex = reader.GetOrdinal("ReviewedById");
            var reviewedById = reader.IsDBNull(reviewedByIdindex) ? (int?)null : reader.GetInt32(reviewedByIdindex);


            var reviewedAtIndex = reader.GetOrdinal("ReviewedAt");
            var reviewedAt = reader.IsDBNull(reviewedAtIndex) ? (DateTime?)null : reader.GetDateTime(reviewedAtIndex);

            return AttendanceCorrection.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                attendanceLogId,
                reader.GetDateTime(reader.GetOrdinal("RequestedClockIn")),
                reader.GetDateTime(reader.GetOrdinal("RequestedClockOut")),
                (AttendanceCorrectionsStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                reader.GetString(reader.GetOrdinal("Reason")),
                reviewedById,
                reviewedAt
                );
        }
    }
}