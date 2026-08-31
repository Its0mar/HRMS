using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Application.Features.Dashboard.Dtos;
using HRMS.Application.Features.Dashboard.EmployeeDasboard;
using HRMS.Domain.Entities.Leaves;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS.Infrastructure.Mappers.Dashboard
{
    public static class EmployeeDashboardResponseMapper
    {
        public static async Task<EmployeeDashboardResponse?> MapAsync(SqlDataReader reader, CancellationToken ct)
        {
            // 1. Result Set 1: Today's Attendance Log
            TodayAttendanceDto? todayAttendance = null;
            if (await reader.ReadAsync(ct))
            {
                var clockOutIdx = reader.GetOrdinal("ClockOut");
                var clockOut = reader.IsDBNull(clockOutIdx)
                    ? null
                    : reader.GetDateTime(clockOutIdx).ToShortTimeString();

                var totalMinIdx = reader.GetOrdinal("TotalMinutes");
                var totalMinutes = reader.IsDBNull(totalMinIdx)
                    ? (int?)null
                    : reader.GetInt32(totalMinIdx);

                todayAttendance = new TodayAttendanceDto(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetFieldValue<DateOnly>(reader.GetOrdinal("Date")),
                    reader.GetDateTime(reader.GetOrdinal("ClockIn")).ToShortTimeString(),
                    clockOut,
                    converStatus(reader.GetInt32(reader.GetOrdinal("Status"))),
                    totalMinutes,
                    reader.GetInt32(reader.GetOrdinal("LateMinutes"))
                );
            }

            // 2. Result Set 2: Leave Balances Cards
            var leaveBalances = new List<MyLeaveBalancesResponse>();
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    leaveBalances.Add(new MyLeaveBalancesResponse(
                        reader.GetInt32(reader.GetOrdinal("LeaveTypeId")),
                        reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                        reader.GetBoolean(reader.GetOrdinal("IsPaid")),
                        reader.GetBoolean(reader.GetOrdinal("RequiresApproval")),
                        reader.GetInt32(reader.GetOrdinal("Year")),
                        (int)reader.GetDecimal(reader.GetOrdinal("TotalEntitledDays")),
                        reader.GetDecimal(reader.GetOrdinal("UsedDays")),
                        reader.GetDecimal(reader.GetOrdinal("PendingDays")),
                        reader.GetDecimal(reader.GetOrdinal("RemainingDays")),
                        reader.GetBoolean(reader.GetOrdinal("IsRecordedInDb"))
                    ));
                }
            }

            // 3. Result Set 3: Recent Leave Requests Tracking
            var recentRequests = new List<RecentLeaveRequestDto>();
            if (await reader.NextResultAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    recentRequests.Add(new RecentLeaveRequestDto(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("LeaveTypeName")),
                        reader.GetFieldValue<DateOnly>(reader.GetOrdinal("StartDate")),
                        reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EndDate")),
                        (LeaveRequestStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                    ));
                }
            }

            return new EmployeeDashboardResponse(
                todayAttendance,
                leaveBalances,
                recentRequests
            );
        }
    
        private static string converStatus(int status)
        {
            if (status == 1) return "Present";
            if (status == 2) return "Late";
            if (status == 3 ) return "HalfDay";
            if (status == 4) return "Absent";
            return "OnLeave";
        }
    
    
    }
}
