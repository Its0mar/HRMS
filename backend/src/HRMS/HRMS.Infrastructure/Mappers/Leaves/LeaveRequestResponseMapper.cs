using HRMS.Application.Features.Leaves.LeaveRequests;
using HRMS.Domain.Entities.Leaves;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Leaves
{
    public static class LeaveRequestResponseMapper
    {
        public static LeaveRequestResponse Map(SqlDataReader reader)
        {
            var rejectionReasonIndex = reader.GetOrdinal("RejectionReason");
            var rejectionReason = reader.IsDBNull(rejectionReasonIndex) ? (string?)null : reader.GetString(rejectionReasonIndex);

            return new LeaveRequestResponse(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                reader.GetDateTime(reader.GetOrdinal("StartDate")),
                reader.GetDateTime(reader.GetOrdinal("EndDate")),
                reader.GetDecimal(reader.GetOrdinal("TotalDays")),
                reader.GetString(reader.GetOrdinal("Reason")),
                (LeaveRequestStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                rejectionReason
                );
        }
    }
}