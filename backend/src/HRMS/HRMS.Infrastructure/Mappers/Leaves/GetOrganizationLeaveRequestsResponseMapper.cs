using HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests;
using HRMS.Domain.Entities.Leaves;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Leaves
{
    public static class GetOrganizationLeaveRequestsResponseMapper
    {
        public static GetOrganizationLeaveRequestsResponse Map(SqlDataReader reader)
        {
            var reviewdByEmployeeIndex = reader.GetOrdinal("ReviewdByEmployee");
            var reviewdByEmployee = reader.IsDBNull(reviewdByEmployeeIndex) ? (string?)null : reader.GetString(reviewdByEmployeeIndex);

            var ReviewdAtIndex = reader.GetOrdinal("ReviewedAt");
            var reviewdAt = reader.IsDBNull(ReviewdAtIndex) ? (DateTime?)null : reader.GetDateTime(ReviewdAtIndex);

            var rejectionReasonIndex = reader.GetOrdinal("RejectionReason");
            var rejectionReason = reader.IsDBNull(rejectionReasonIndex) ? (string?)null : reader.GetString(rejectionReasonIndex);



            return new GetOrganizationLeaveRequestsResponse(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                reader.GetString(reader.GetOrdinal("EmployeeName")),
                reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                reader.GetString(reader.GetOrdinal("LeaveName")),
                reader.GetFieldValue<DateOnly>(reader.GetOrdinal("StartDate")),
                reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EndDate")),
                reader.GetDecimal(reader.GetOrdinal("TotalDays")),
                reader.GetString(reader.GetOrdinal("Reason")),
                (LeaveRequestStatus) reader.GetInt32(reader.GetOrdinal("Status")),
                reviewdByEmployee,
                reviewdAt,
                rejectionReason
                );
        }
    }
}