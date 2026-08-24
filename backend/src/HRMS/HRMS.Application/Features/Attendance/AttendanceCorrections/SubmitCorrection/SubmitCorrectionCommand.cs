using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.AttendanceCorrections.SubmitCorrection
{
    public sealed record SubmitCorrectionCommand(
        int? AttendanceLogId,
        DateTime RequestedClockIn,
        DateTime RequestedClockOut,
        string Reason)
        : ICommand<bool>;
}