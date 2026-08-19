
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

        public AttendanceLog(int employeeId, int workScheduleId, int organizationId, AttendanceStatus status, int lateMinutes) 
        {
            EmployeeId = employeeId;
            WorkScheduleId = workScheduleId;
            OrganizationId = organizationId;
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = status;
            LateMinutes = lateMinutes;
            ClockIn = DateTime.UtcNow;
        }

        public static AttendanceLog Restore(int? id, int employeeId, int workScheduleId, int organizationId, DateOnly date, DateTime clockIn, DateTime? clockOut, AttendanceStatus status, int? totalMinutes, int lateMinutes, int overtimeMinutes, string? notes)
        {
            return new AttendanceLog(employeeId, workScheduleId, organizationId, status, lateMinutes)
            {
                Id = id,
                ClockIn = clockIn,
                ClockOut = clockOut,
                TotalMinutes = totalMinutes,
                OvertimeMinutes = overtimeMinutes,
                Notes = notes,
                Date = date,
            };
        }
    }
}
