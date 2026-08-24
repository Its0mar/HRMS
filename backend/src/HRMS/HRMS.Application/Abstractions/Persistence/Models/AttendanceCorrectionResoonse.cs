namespace HRMS.Application.Abstractions.Persistence.Models
{
    public record AttendanceCorrectionResoonse(
        int Id,
        int? AttendanceLogId,
        DateTime RequestedClockIn,
        DateTime RequestedClockOut,
        int Status,
        string Reason,
        string EmployeeNumber,
        string EmployeeName
        );
}
