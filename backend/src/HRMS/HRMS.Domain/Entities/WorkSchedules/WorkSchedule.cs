namespace HRMS.Domain.Entities.WorkSchedules
{
    public class WorkSchedule
    {
        public int? Id { get; private set; }
        public int OrganizationId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int GracePeriodMinutes { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDefault { get; private set; }

        private readonly List<WorkScheduleDay> _days = [];
        public IReadOnlyCollection<WorkScheduleDay> Days => _days.AsReadOnly();


        public WorkSchedule(int organizationId, string name, int gracePeriodMinutes, IEnumerable<WorkScheduleDay> days,bool isDefault = false)
        {

            OrganizationId = organizationId;
            Name = name;
            GracePeriodMinutes = gracePeriodMinutes;
            IsDefault = isDefault;
            _days.AddRange(days);
        }

        public static WorkSchedule Restore(int id, int organizationId, string name, int gracePeriodMinutes, IEnumerable<WorkScheduleDay> days, bool isActive, bool isDefault)
        {
            return new WorkSchedule(organizationId, name, gracePeriodMinutes,days, isDefault)
            {
                Id = id,
                IsActive = isActive
            };
        }

        public void AddOrUpdateWorkScheduleDay(WorkScheduleDay shiftDay)
        {
            var existingDay = _days.FirstOrDefault(d => d.WorkDay == shiftDay.WorkDay);
            if (existingDay != null)
            {
                existingDay.Update(shiftDay.IsWorkingDay, shiftDay.StartTime, shiftDay.EndTime, shiftDay.MinimumMinutesPerDay, shiftDay.BreakDurationMinutes);
                return;
            }
            
            _days.Add(shiftDay);
        }

        public void UpdateWorkSchedule(string name, int gracePeriodMinutes, bool isDefault, List<WorkScheduleDay> workScheduleDays)
        {
            Name = name;
            GracePeriodMinutes = gracePeriodMinutes;
            IsDefault = isDefault;

            workScheduleDays.ForEach(AddOrUpdateWorkScheduleDay);
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
