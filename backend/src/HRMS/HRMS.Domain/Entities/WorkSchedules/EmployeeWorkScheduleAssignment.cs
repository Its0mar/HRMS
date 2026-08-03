namespace HRMS.Domain.Entities.WorkSchedules
{
    public sealed class EmployeeWorkScheduleAssignment
    {
        public int EmployeeId { get; private set; }
        public int WorkScheduleId { get; private set; }
        public DateOnly EffectiveFrom { get; private set; }
        public DateOnly? EffectiveTo { get; private set; }

        public EmployeeWorkScheduleAssignment(int employeeId, int workScheduleId, DateOnly effectiveFrom, DateOnly? effectiveTo = null)
        {
            EmployeeId = employeeId;
            WorkScheduleId = workScheduleId;
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
        }

        public void End(DateOnly effectiveTo)
        {
            EffectiveTo = effectiveTo;
        }
    }
}
