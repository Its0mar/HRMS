using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Features.Attendance.SubmitCorrection
{
    public sealed class SubmitCorrectionHandler(IAttendanceRepository attendanceRepository, ICurrentUser currentUser)
        : ICommandHandler<SubmitCorrectionCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(SubmitCorrectionCommand command, CancellationToken cancellationToken)
        {
            var attendanceCorrection = new AttendanceCorrection(currentUser.OrganizationId,
                                                                currentUser.EmployeeId,
                                                                command.AttendanceLogId,
                                                                command.RequestedClockIn,
                                                                command.RequestedClockOut,
                                                                command.Reason);

            var result = await attendanceRepository.CreateAttendanceCorrectionAsync(attendanceCorrection, cancellationToken);

            return result > 0;
        }
    }
}
