using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Domain.Entities.Attendance;
using HRMS.Infrastructure.Mappers.Attendance;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    public sealed class AttendanceCorrectionsRepository(ISqlExecutor sqlExecutor) : IAttendanceCorrectionsRepository
    {
        public async Task<IReadOnlyList<AttendanceCorrectionResponse>> GetOrganizationRecordsAsync(int organizationId, int status, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "AttendanceCorrections_GetOrganizationRecords",
                Map,
                cancellationToken,
                Int("@OrganizationId", organizationId),
                Int("@Status", status)
                );
        }

        public async Task<AttendanceCorrection?> GetByIdAsync(int Id, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.AttendanceCorrections_GetById",
                AttendanceCorrectionMapper.Map,
                cancellationToken,
                Int("Id", Id)
                );
        }
        
        public async Task ApproveOrRejectCorrection(AttendanceCorrection correction, CancellationToken cancellationToken)
        {
            await sqlExecutor.ExecuteAsync(
                "AttendanceCorrections_ApproveCorrection",
                cancellationToken,
                NullableInt("Id", correction.Id),
                Int("OrganizationId", correction.OrganizationId),
                Int("@Status", (int) correction.Status),
                NullableInt("@ReviewedById", correction.ReviewedById),
                NullableDateTime2("@ReviewedAt", correction.ReviewedAt)
                );
        }


        private AttendanceCorrectionResponse Map(SqlDataReader reader)
        {
            var AttendanceLogIdIndex = reader.GetOrdinal("AttendanceLogId");
            var attendanceLogId = reader.IsDBNull(AttendanceLogIdIndex) ? (int?) null: reader.GetInt32(AttendanceLogIdIndex);

            return new AttendanceCorrectionResponse(
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