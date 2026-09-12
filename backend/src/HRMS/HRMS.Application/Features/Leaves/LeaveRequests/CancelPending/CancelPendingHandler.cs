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
            var leaveRequest = await leaveRepository.GetRequestByIdAsync(command.Id, command.OrganizationId, cancellationToken);
            if (leaveRequest is null)
            {
                return Error.NotFound("LeaveRequest.NotFound", "Leave request not found.");
            }

            var cancelResult = leaveRequest.Cancel();
            if (cancelResult.IsError)
            {
                return cancelResult.Errors;
            }

            var result = await leaveRepository.CancelLeaveRequestAsync(command.Id, command.EmployeeId, command.OrganizationId, cancellationToken);
            return result ? true : Error.Failure(description: "Failed to cancel leave request.");
        }
    }
}
