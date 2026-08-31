
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Application.Features.Dashboard.Dtos;

namespace HRMS.Application.Features.Dashboard.EmployeeDasboard
{
    public record EmployeeDashboardResponse(
        TodayAttendanceDto? TodayAttendance,
        IReadOnlyList<MyLeaveBalancesResponse> LeaveBalances,
        IReadOnlyList<RecentLeaveRequestDto> RecentRequests
);
}
