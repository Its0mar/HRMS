using HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests;
using HRMS.Domain.Entities.Leaves;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Leaves
{
    public static class GetMyLeaveRequestResponseMapper
    {
        public static GetMyLeaveRequestResponse Map(SqlDataReader reader)
        {
            var rejectionReasonIndex = reader.GetOrdinal("RejectionReason");
            var rejectionReason = reader.IsDBNull(rejectionReasonIndex) ? (string?)null : reader.GetString(rejectionReasonIndex);

            return new GetMyLeaveRequestResponse(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                reader.GetFieldValue<DateOnly>(reader.GetOrdinal("StartDate")),
                reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EndDate")),
                reader.GetDecimal(reader.GetOrdinal("TotalDays")),
                reader.GetString(reader.GetOrdinal("Reason")),
                (LeaveRequestStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                rejectionReason
                );
        }
    }
}