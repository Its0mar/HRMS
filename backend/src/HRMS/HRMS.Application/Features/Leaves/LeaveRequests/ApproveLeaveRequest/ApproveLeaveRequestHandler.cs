using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leaves.LeaveRequests.ApproveLeaveRequest
{
    public sealed class ApproveLeaveRequestHandler(
        ICurrentUser currentUser, 
        ILeaveRepository leaveRepository,
        ILogger<ApproveLeaveRequestHandler> logger)
        : ICommandHandler<ApproveLeaveRequestCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(ApproveLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var leaveRequest = await leaveRepository.GetRequestByIdAsync(command.RequestId, currentUser.OrganizationId, cancellationToken);
            if (leaveRequest is null)
            {
                logger.LogWarning("Leave request {RequestId} not found for organization {OrganizationId}", command.RequestId, currentUser.OrganizationId);
                return Error.NotFound("LeaveRequest.NotFound", "Leave request not found.");
            }

            var approveResult = leaveRequest.Approve(currentUser.EmployeeId);
            if (approveResult.IsError)
            {
                logger.LogWarning("Failed to approve leave request {RequestId}: {ErrorCode}", command.RequestId, approveResult.FirstError.Code);
                return approveResult.Errors;
            }

            var result = await leaveRepository.ApproveLeaveRequestAsync(
               command.RequestId,
               currentUser.OrganizationId,
               currentUser.EmployeeId,
               cancellationToken);

            if (!result)
            {
                return Error.Conflict("Leave.ApproveFailed", "Could not approve request. It may already be processed.");
            }

            logger.LogInformation("Leave request {RequestId} approved by manager {ManagerId} for employee {EmployeeId}", command.RequestId, currentUser.EmployeeId, leaveRequest.EmployeeId);
            return true;
        }
    }
}
