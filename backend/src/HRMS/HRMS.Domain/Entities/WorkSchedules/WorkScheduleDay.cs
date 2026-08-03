using HRMS.Domain.Entities.WorkSchedules.Enums;

namespace HRMS.Domain.Entities.WorkSchedules
{
    public sealed class WorkScheduleDay
    {
        public int Id { get; private set; }
        public int WorkScheduleId { get; private set; }
        public WorkDay WorkDay { get; private set; }
        public bool IsWorkingDay { get; private set; }
        public TimeOnly? StartTime { get; private set; }
        public TimeOnly? EndTime { get; private set; }
        public TimeSpan? MinimumHoursPerDay { get; private set; }
        public int BreakDurationMinutes { get; private set; }

        public WorkScheduleDay(WorkDay workDay, bool isWorkingDay, TimeOnly? startTime, TimeOnly? endTime, TimeSpan? minimumHoursPerDay, int breakDurationMinutes)
        {
            WorkDay = workDay;
            IsWorkingDay = isWorkingDay;
            StartTime = startTime;
            EndTime = endTime;
            MinimumHoursPerDay = minimumHoursPerDay;
            BreakDurationMinutes = breakDurationMinutes;
        }

        public static WorkScheduleDay Restore(int Id,int workScheduleId, WorkDay workDay, bool isWorkingDay, TimeOnly? startTime, TimeOnly? endTime, TimeSpan? minimumHoursPerDay, int breakDurationMinutes)
        {
            return new WorkScheduleDay(workDay, isWorkingDay, startTime, endTime, minimumHoursPerDay, breakDurationMinutes)
            {
                Id = Id,
                WorkScheduleId = workScheduleId
            };
        }

        public void Update(bool isWorkingDay, TimeOnly? startTime, TimeOnly? endTime, TimeSpan? minimumHoursPerDay, int breakDurationMinutes)
        {
            IsWorkingDay = isWorkingDay;
            StartTime = startTime;
            EndTime = endTime;
            MinimumHoursPerDay = minimumHoursPerDay;
            BreakDurationMinutes = breakDurationMinutes;
        }
    }
}
