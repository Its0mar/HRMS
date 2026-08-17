
namespace HRMS.Domain.Entities.Attendance
{
    public  class AttendanceLog
    {
        public int? Id { get; private set; }
        public int EmployeeId { get; private set; }
        public int WorkScheduleId { get; private set; }
        public int OrganizationId { get; private set; }
        public DateOnly Date {  get; private set; } 
        public DateTime ClockIn { get; private set; }
        public DateTime? ClockOut { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public int? TotalMinutes { get; private set; }
        public int LateMinutes { get; private set; } = 0;
        public int OvertimeMinutes { get; private set; } = 0;
        public string? Notes { get; private set; }

        public AttendanceLog(int employeeId, int workScheduleId, int organizationId) 
        {
            EmployeeId = employeeId;
            WorkScheduleId = workScheduleId;
            OrganizationId = organizationId;
            Date = new DateOnly();
            Status = AttendanceStatus.Present;
            ClockIn = DateTime.UtcNow;
        }

        public AttendanceLog Restore(int? id, int employeeId, int workScheduleId, int organizationId, DateOnly date, DateTime clockIn, DateTime? clockOut, AttendanceStatus status, int? totalMinutes, int lateMinutes, int overtimeMinutes, string? notes)
        {
            return new AttendanceLog(employeeId, workScheduleId, organizationId)
            {
                Id = id,
                ClockOut = clockOut,
                TotalMinutes = totalMinutes,
                LateMinutes = lateMinutes,
                OvertimeMinutes = overtimeMinutes,
                Notes = notes
            };
        }
    }
}
