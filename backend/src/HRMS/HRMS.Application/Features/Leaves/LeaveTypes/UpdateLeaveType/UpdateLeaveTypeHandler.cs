using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType
{
    public sealed class UpdateLeaveTypeHandler(
        ILeaveRepository leaveRepository,
        ICurrentUser currentUser) : ICommandHandler<UpdateLeaveTypeCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var leaveType = await leaveRepository.GetTypeByIdAsync(command.Id, currentUser.OrganizationId, cancellationToken);
            if (leaveType is null)
            {
                return Error.NotFound("LeaveType.NotFound", "The specified leave type does not exist.");
            }

            leaveType.Update(
                command.Name.Trim(),
                command.Code.Trim().ToUpper(),
                command.DefaultDaysPerYear,
                command.IsPaid,
                command.RequiresApproval);

            return await leaveRepository.UpdateLeaveTypeAsync(leaveType, cancellationToken);
        }
    }
}
