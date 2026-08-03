using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Domain.Entities.WorkSchedules;
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
                    day.MinimumHoursPerDay.HasValue
                        ? (short)day.MinimumHoursPerDay.Value.TotalMinutes
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
//@OrganizationId INT,
//@Name VARCHAR(100),
//@GracePeriodMinutes SMALLINT,
//@IsDefault BIT,
//@Days dbo.WorkScheduleDayInput READONLY
