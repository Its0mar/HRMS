using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    public sealed class AttendanceCorrectionsRepository(ISqlExecutor sqlExecutor) : IAttendanceCorrectionsRepository
    {
        public async Task<IReadOnlyList<OrganizationAttendanceCorrection>> GetOrganizationRecordsAsync(int organizationId, int status, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "AttendanceCorrections_GetOrganizationRecords",
                Map,
                cancellationToken,
                Int("@OrganizationId", organizationId),
                Int("@Status", status)
                );
        }

        private OrganizationAttendanceCorrection Map(SqlDataReader reader)
        {
            var AttendanceLogIdIndex = reader.GetOrdinal("AttendanceLogId");
            var attendanceLogId = reader.IsDBNull(AttendanceLogIdIndex) ? (int?) null: reader.GetInt32(AttendanceLogIdIndex);

            return new OrganizationAttendanceCorrection(
                reader.GetInt32(reader.GetOrdinal("Id")),
                attendanceLogId,
                reader.GetDateTime(reader.GetOrdinal("RequestedClockIn")),
                reader.GetDateTime(reader.GetOrdinal("RequestedClockOut")),
                reader.GetInt32(reader.GetOrdinal("Status")),
                reader.GetString(reader.GetOrdinal("Reason")),
                reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                reader.GetString(reader.GetOrdinal("EmployeeName"))
                );
        }
    }
}