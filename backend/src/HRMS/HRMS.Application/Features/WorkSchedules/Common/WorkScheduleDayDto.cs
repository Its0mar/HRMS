using HRMS.Domain.Entities.WorkSchedules;
using HRMS.Domain.Entities.WorkSchedules.Enums;

namespace HRMS.Application.Features.WorkSchedules.Common
{
    public record WorkScheduleDayDto(
        WorkDay WorkDay,
        bool IsWorkingDay,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        short? MinimumMinutesPerDay,
        short BreakDurationMinutes = 0
    );

    public static class Mapper {
        public static WorkScheduleDay ToWorkScheduleDay(this WorkScheduleDayDto DayDto)
        {
            return new WorkScheduleDay(DayDto.WorkDay, DayDto.IsWorkingDay, DayDto.StartTime, DayDto.EndTime, DayDto.MinimumMinutesPerDay, DayDto.BreakDurationMinutes);
        }
    }
}
