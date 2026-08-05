using HRMS.Domain.Entities.WorkSchedules.Enums;


namespace HRMS.Application.Features.WorkSchedules.GetWorkScheduleWithDays
{
    public record WorkScheduleWithDaysResponse(
       int Id,
       string Name,
       int GracePeriodMinutes,
       bool IsDefault,
       bool IsActive,
       List<WorkScheduleDayResponseDto> WorkScheduleDays);


    public record WorkScheduleDayResponseDto(
        WorkDay WorkDay,
        bool IsWorkingDay,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        short? MinimumMinutesPerDay,
        short BreakDurationMinutes);
}
