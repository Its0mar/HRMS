namespace HRMS.Application.Abstractions.Persistence.Models
{
    public record AttendanceCorrectionResponse(
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
