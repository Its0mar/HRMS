using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveRequests.RejectLeaveRequest
{
    public sealed class RejectLeaveRequestHandler(
    ILeaveRepository leaveRepository,
    ICurrentUser currentUser) : ICommandHandler<RejectLeaveRequestCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(RejectLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.RejectionReason))
            {
                return Error.Validation("Leave.ReasonRequired", "A rejection reason is required.");
            }
            var result = await leaveRepository.RejectLeaveRequestAsync(
                command.RequestId,
                currentUser.OrganizationId,
                currentUser.EmployeeId,
                command.RejectionReason.Trim(),
                cancellationToken);
            if (!result)
            {
                return Error.Conflict("Leave.RejectFailed", "Could not reject request. It may already be processed.");
            }
            return true;
        }
    }
}
