namespace HRMS.Api.Contracts.Attendance;

public record SubmitCorrectionRequest(
    int? AttendanceLogId,
    DateTime RequestedClockIn,
    DateTime RequestedClockOut,
    string Reason
);
