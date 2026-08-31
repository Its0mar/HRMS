namespace HRMS.Application.Features.Dashboard.AdminDashboard
{
    public record AdminDashboardKpiDto(
        int TotalEmployees,
        int PresentToday,
        int LateToday,
        int OnLeaveToday,
        int PendingCorrectionsCount,
        int PendingLeaveRequestsCount
    );

    public record PendingLeaveRequestSummaryDto(
        int Id,
        string EmployeeName,
        string EmployeeNumber,
        string LeaveTypeName,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal TotalDays,
        string Reason
    );

    public record PendingCorrectionSummaryDto(
        int Id,
        string EmployeeName,
        string EmployeeNumber,
        string Reason
    );

    public record AdminDashboardResponse(
        AdminDashboardKpiDto Kpis,
        IReadOnlyList<PendingLeaveRequestSummaryDto> PendingLeaves,
        IReadOnlyList<PendingCorrectionSummaryDto> PendingCorrections
    );
}
