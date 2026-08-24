using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Features.Attendance.AttendanceCorrections.AttendanceCorrectionApprove
{
    public sealed class AttendanceCorrectionApproveHandler
        (IAttendanceCorrectionsRepository attendanceCorrectionsRepository,
        ICurrentUser currentUser)
        : ICommandHandler<AttendanceCorrectionApproveCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(AttendanceCorrectionApproveCommand command, CancellationToken cancellationToken)
        {
            var attendanceCorrection  = await attendanceCorrectionsRepository.GetByIdAsync(command.Id, cancellationToken);

            if (attendanceCorrection is null)
            {
                return Error.NotFound(description: "Attendace correction with this id is not found");
            }

            if (attendanceCorrection.Status != AttendanceCorrectionsStatus.Pending)
            {
                return Error.Conflict(description: "Staff already responsed to this request");
            }

            if (command.Approve) attendanceCorrection.Approve(currentUser.EmployeeId);
            else attendanceCorrection.Reject(currentUser.EmployeeId);

            await attendanceCorrectionsRepository.ApproveOrRejectCorrection(attendanceCorrection, cancellationToken);

            return true;
        }
    }
}
