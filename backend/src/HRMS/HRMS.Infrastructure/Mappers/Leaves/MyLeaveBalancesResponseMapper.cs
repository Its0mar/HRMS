using HRMS.Application.Abstractions.Persistence.Models;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Leaves
{
    public static class MyLeaveBalancesResponseMapper
    {
        public static MyLeaveBalancesResponse Map(SqlDataReader reader)
        {
            return new MyLeaveBalancesResponse(
                reader.GetInt32(reader.GetOrdinal("LeaveTypeId")),
                reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                reader.GetBoolean(reader.GetOrdinal("IsPaid")),
                reader.GetBoolean(reader.GetOrdinal("RequiresApproval")),
                reader.GetInt32(reader.GetOrdinal("Year")),
                reader.GetInt32(reader.GetOrdinal("TotalEntitledDays")),
                reader.GetInt32(reader.GetOrdinal("UsedDays")),
                reader.GetInt32(reader.GetOrdinal("PendingDays")),
                reader.GetInt32(reader.GetOrdinal("RemainingDays"))
                );
        }
    }
}
