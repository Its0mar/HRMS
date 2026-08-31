using HRMS.Application.Features.Dashboard.AdminDashboard;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Dashboard
{
    public static class AdminDashboardResponseMapper
    {
        public static async Task<AdminDashboardResponse?> MapAsync(SqlDataReader reader, CancellationToken ct)
        {
            // 1. Result Set 1: KPIs & Counts
            AdminDashboardKpiDto kpis = new(0, 0, 0, 0, 0, 0);
            if (await reader.ReadAsync(ct))
            {
                kpis = new AdminDashboardKpiDto(
                    reader.GetInt32(reader.GetOrdinal("TotalEmployees")),
                    reader.GetInt32(reader.GetOrdinal("PresentToday")),
                    reader.GetInt32(reader.GetOrdinal("LateToday")),
                    reader.GetInt32(reader.GetOrdinal("OnLeaveToday")),
                    reader.GetInt32(reader.GetOrdinal("PendingCorrectionsCount")),
                    reader.GetInt32(reader.GetOrdinal("PendingLeaveRequestsCount"))
                );
            }

            // 2. Result Set 2: Top 5 Pending Leave Requests
            var pendingLeaves = new List<PendingLeaveRequestSummaryDto>();
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    pendingLeaves.Add(new PendingLeaveRequestSummaryDto(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("EmployeeName")),
                        reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                        reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                        reader.GetFieldValue<DateOnly>(reader.GetOrdinal("StartDate")),
                        reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EndDate")),
                        reader.GetDecimal(reader.GetOrdinal("TotalDays")),
                        reader.GetString(reader.GetOrdinal("Reason"))
                    ));
                }
            }

            // 3. Result Set 3: Top 5 Pending Attendance Corrections
            var pendingCorrections = new List<PendingCorrectionSummaryDto>();
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    pendingCorrections.Add(new PendingCorrectionSummaryDto(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("EmployeeName")),
                        reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                        reader.GetString(reader.GetOrdinal("Reason"))
                    ));
                }
            }

            return new AdminDashboardResponse(kpis, pendingLeaves, pendingCorrections);
        }
    }
}
