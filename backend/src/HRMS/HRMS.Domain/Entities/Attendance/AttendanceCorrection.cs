namespace HRMS.Domain.Entities.Attendance
{
    public class AttendanceCorrection
    {
        public int? Id {  get; private set; }
        public int OrganizationId { get; private set; }
        public int EmployeeId { get; private set; }
        public int? AttendanceLogId { get; private set; }
        public DateTime RequestedClockIn { get; private set; }
        public DateTime RequestedClockOut { get; private set; }
        public AttendanceCorrectionsStatus Status { get; private set; } = AttendanceCorrectionsStatus.Pending;
        public string Reason { get; private set; } = string.Empty;
        public int? ReviewedById { get; private set; }
        public DateTime? ReviewedAt { get; private set; }

        public AttendanceCorrection(int organizationId, int employeeId, int? attendanceLogId, DateTime requestedClockIn, DateTime requestedClockOut, string reason)
        {
            EmployeeId = employeeId;
            OrganizationId = organizationId;
            AttendanceLogId = attendanceLogId;
            RequestedClockIn = requestedClockIn;
            RequestedClockOut = requestedClockOut;
            Reason = reason;
        }

        public static AttendanceCorrection Restore(int? id, int organizationId, int employeeId, int? attendanceLogId, DateTime requestedClockIn, DateTime requestedClockOut, AttendanceCorrectionsStatus status, string reason, int? reviewedById, DateTime? reviewedAt)
        {
            return new AttendanceCorrection(organizationId, employeeId, attendanceLogId, requestedClockIn, requestedClockOut, reason)
            {
                Id = id,
                Status = status,
                Reason = reason,
                ReviewedById = reviewedById,
                ReviewedAt = reviewedAt
            };

        }
    }
}
