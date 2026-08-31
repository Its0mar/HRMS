using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveRequests.CancelPending
{
    public sealed class CancelPendingHandler(ILeaveRepository leaveRepository)
        : ICommandHandler<CancelPendingCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(CancelPendingCommand command, CancellationToken cancellationToken)
        {
            var result = await leaveRepository.CancelLeaveRequestAsync(command.Id, command.EmployeeId, command.OrganizationId, cancellationToken);

            return result ? true : Error.Failure(description: "failed to cancel request");
        }
    }
}
