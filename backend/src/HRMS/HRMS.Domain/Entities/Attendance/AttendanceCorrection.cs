namespace HRMS.Domain.Entities.Attendance
{
    public class AttendanceCorrection
    {
        public int? Id {  get; private set; }
        public int AttendanceLogId { get; private set; }
        public DateTime? RequestedClockIn { get; private set; }
        public DateTime? RequestedClockOut { get; private set; }
        public AttendanceCorrectionsStatus Status { get; private set; } = AttendanceCorrectionsStatus.Pending;
        public string Reason { get; private set; } = string.Empty;
        public int? ReviewedById { get; private set; }
        public DateTime? ReviewedAt { get; private set; }

        public AttendanceCorrection(int attendanceLogId, DateTime? requestedClockI, DateTime? requestedClockOut, string reason)
        {
            AttendanceLogId = attendanceLogId;
            RequestedClockIn = requestedClockI;
            RequestedClockOut = requestedClockOut;
            Reason = reason;
        }

        public AttendanceCorrection Restore(int? id, int attendanceLogId, DateTime? requestedClockIn, DateTime? requestedClockOut, AttendanceCorrectionsStatus status, string reason, int? reviewedById, DateTime? reviewedAt)
        {
            return new AttendanceCorrection(attendanceLogId, requestedClockIn, requestedClockOut, reason)
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
