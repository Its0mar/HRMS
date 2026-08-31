using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveRequests.ApproveLeaveRequest
{
    public sealed class ApproveLeaveRequestHandler(ICurrentUser currentUser, ILeaveRepository leaveRepository)
        : ICommandHandler<ApproveLeaveRequestCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(ApproveLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var result = await leaveRepository.ApproveLeaveRequestAsync(
               command.RequestId,
               currentUser.OrganizationId,
               currentUser.EmployeeId,
               cancellationToken);

            if (!result)
            {
                return Error.Conflict("Leave.ApproveFailed", "Could not approve request. It may already be processed.");
            }
            return true;
        }
    }
}
