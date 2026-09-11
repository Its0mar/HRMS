using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType
{
    public sealed class UpdateLeaveTypeHandler(
        ILeaveRepository leaveRepository) : ICommandHandler<UpdateLeaveTypeCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var leaveType = await leaveRepository.GetTypeByIdAsync(command.Id, command.OrganizationId, cancellationToken);
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

            var result =  await leaveRepository.UpdateLeaveTypeAsync(leaveType, cancellationToken);

            if (!result) return Error.Failure("LeaveType.UpdateFailed", "Failed to update the leave type.");  

            return true;
        }
    }
}
