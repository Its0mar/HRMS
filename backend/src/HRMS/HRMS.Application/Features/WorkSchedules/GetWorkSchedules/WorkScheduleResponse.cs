namespace HRMS.Application.Features.WorkSchedules.GetWorkSchedules
{
    public record WorkScheduleResponse(
        int Id,
        string Name,
        int GracePeriodMinutes,
        bool IsDefault,
        bool IsActive);
}
