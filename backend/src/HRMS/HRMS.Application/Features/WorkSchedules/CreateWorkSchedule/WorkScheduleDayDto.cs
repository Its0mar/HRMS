using HRMS.Domain.Entities.WorkSchedules.Enums;

namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedules
{
    public record WorkScheduleDayDto(
        WorkDay WorkDay,
        bool IsWorkingDay,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        short? MinimumMinutesPerDay,
        short BreakDurationMinutes = 0
     );
}