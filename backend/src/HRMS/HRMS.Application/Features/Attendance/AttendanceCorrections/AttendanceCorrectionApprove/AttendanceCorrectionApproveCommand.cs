using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.AttendanceCorrections.AttendanceCorrectionApprove
{
    public record AttendanceCorrectionApproveCommand(
        int Id,
        bool Approve
        ) : ICommand<bool>;
}
