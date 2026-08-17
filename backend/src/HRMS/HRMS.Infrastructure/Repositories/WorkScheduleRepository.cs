using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.WorkSchedules;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Repositories
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public WorkScheduleRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task<int> CreateWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@OrganizationId", workSchedule.OrganizationId),
                new SqlParameter("@Name", workSchedule.Name),
                new SqlParameter("@GracePeriodMinutes", workSchedule.GracePeriodMinutes),
                new SqlParameter("@IsDefault", workSchedule.IsDefault)
            };

            var daysParameter = CreateWorkScheduleDayDataTable(workSchedule.Days);
            parameters.Add(daysParameter);

            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "WorkSchedule_Create",
                cancellationToken,
                parameters.ToArray()
                );
        }

        public async Task<int> UpdateWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken cancellationToken)
        {

            var daysParameter = CreateWorkScheduleDayDataTable(workSchedule.Days);

            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "WorkSchedules_Update",
                cancellationToken,
                new SqlParameter("@Id", workSchedule.Id),
                new SqlParameter("@OrganizationId", workSchedule.OrganizationId),
                new SqlParameter("@Name", workSchedule.Name),
                new SqlParameter("@GracePeriodMinutes", workSchedule.GracePeriodMinutes),
                new SqlParameter("@IsDefault", workSchedule.IsDefault),
                daysParameter
                );
        }

        public async Task<WorkSchedule?> GetWorkScheduleByIdAsync(int id, int organizationId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryMultipleAsync(
                "WorkSchedules_GetById",
                WorkScheduleMapper.MapWithDaysAsync,
                cancellationToken,
                new SqlParameter("@Id", id),
                new SqlParameter("@OrganizationId", organizationId)
                );
        }

        public async Task<bool> AssignEmployeeAsync(int employeeId, int workScheduleId, DateTime effectiveFrom, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.ExecuteAsync(
                "WorkSchedule_AssignEmployee",
                cancellationToken,
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@WorkScheduleId", workScheduleId),
                new SqlParameter("@EffectiveFrom", effectiveFrom)
                ) > 0;
        }

        public async Task<IEnumerable<WorkSchedule>> GetWorkSchedulesByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryAsync(
                "WorkSchedules_GetByOrganizationId",
                WorkScheduleMapper.Map,
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId)
                );
        }

        public async Task<bool> NameExistAsync(string name, int organizationId, int? excludeId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.ExecuteScalarBoolAsync(
                "WorkSchedules_Name_Exist",
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId),
                new SqlParameter("@ExcludeId", excludeId),
                new SqlParameter("@Name", name)
                );
        }
        private static SqlParameter CreateWorkScheduleDayDataTable(IEnumerable<WorkScheduleDay> days)
        {
            var daysTable = new DataTable();
            daysTable.Columns.Add("WorkDay", typeof(byte));
            daysTable.Columns.Add("IsWorkingDay", typeof(bool));
            daysTable.Columns.Add("StartTime", typeof(TimeSpan));
            daysTable.Columns.Add("EndTime", typeof(TimeSpan));
            daysTable.Columns.Add("MinimumMinutesPerDay", typeof(short));
            daysTable.Columns.Add("BreakDurationMinutes", typeof(short));

            foreach (var day in days)
            {
                daysTable.Rows.Add(
                    (byte)day.WorkDay,
                    day.IsWorkingDay,
                    day.StartTime?.ToTimeSpan() ?? (object)DBNull.Value,
                    day.EndTime?.ToTimeSpan() ?? (object)DBNull.Value,
                    day.MinimumMinutesPerDay.HasValue
                        ? day.MinimumMinutesPerDay
                        : DBNull.Value,
                    (short)day.BreakDurationMinutes
                );
            }

            var daysParameter = new SqlParameter(
                "@Days",
                SqlDbType.Structured)
            {
                TypeName = "dbo.WorkScheduleDayInput",
                Value = daysTable
            };

            return daysParameter;
        }
    }
}