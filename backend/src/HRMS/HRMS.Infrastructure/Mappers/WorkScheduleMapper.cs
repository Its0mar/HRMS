using HRMS.Domain.Entities.WorkSchedules;
using HRMS.Domain.Entities.WorkSchedules.Enums;
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

        public static async Task<WorkSchedule?> MapWithDaysAsync(
            SqlDataReader reader,
            CancellationToken cancellationToken)
        {
            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var organizationId =
                reader.GetInt32(reader.GetOrdinal("OrganizationId"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var gracePeriodMinutes =
                reader.GetInt32(reader.GetOrdinal("GracePeriodMinutes"));
            var isDefault =
                reader.GetBoolean(reader.GetOrdinal("IsDefault"));
            var isActive =
                reader.GetBoolean(reader.GetOrdinal("IsActive"));

            var days = new List<WorkScheduleDay>();

            if (await reader.NextResultAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var startTimeOrdinal =
                        reader.GetOrdinal("StartTime");
                    var endTimeOrdinal =
                        reader.GetOrdinal("EndTime");
                    var minimumMinutesOrdinal =
                        reader.GetOrdinal("MinimumMinutesPerDay");

                    var day = WorkScheduleDay.Restore(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetInt32(reader.GetOrdinal("WorkScheduleId")),
                        (WorkDay)reader.GetByte(reader.GetOrdinal("WorkDay")),
                        reader.GetBoolean(reader.GetOrdinal("IsWorkingDay")),

                        reader.IsDBNull(startTimeOrdinal)
                            ? null
                            : TimeOnly.FromTimeSpan(
                                reader.GetTimeSpan(startTimeOrdinal)),

                        reader.IsDBNull(endTimeOrdinal)
                            ? null
                            : TimeOnly.FromTimeSpan(
                                reader.GetTimeSpan(endTimeOrdinal)),

                        reader.IsDBNull(minimumMinutesOrdinal)
                            ? null
                            : reader.GetInt16(minimumMinutesOrdinal),

                        reader.GetInt16(
                            reader.GetOrdinal("BreakDurationMinutes")));

                    days.Add(day);
                }
            }

            return WorkSchedule.Restore(
                id,
                organizationId,
                name,
                gracePeriodMinutes,
                days,
                isActive,
                isDefault);
        }
    }
}