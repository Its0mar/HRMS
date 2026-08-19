using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.SubmitCorrection
{
    public sealed record SubmitCorrectionCommand(
        int? AttendanceLogId,
        DateTime RequestedClockIn,
        DateTime RequestedClockOut,
        string Reason)
        : ICommand<bool>;
}