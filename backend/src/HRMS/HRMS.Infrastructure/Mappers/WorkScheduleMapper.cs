using HRMS.Domain.Entities.WorkSchedules;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class WorkScheduleMapper
    {
        public static WorkSchedule Map(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var organizationId = reader.GetInt32(reader.GetOrdinal("OrganizationId"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var gracePeriodMinutes = reader.GetInt32(reader.GetOrdinal("GracePeriodMinutes"));
            var isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
            var isDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault"));

            return WorkSchedule.Restore(id, organizationId, name, gracePeriodMinutes, [], isActive, isDefault);
        }
    }
}