using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using LeaveTypeEntity = HRMS.Domain.Entities.Leaves.LeaveType;

namespace HRMS.Application.Features.Leaves.LeaveType.CreateLeaveType
{
    public sealed class CreateLeaveTypeHandler(ILeaveRepository leaveRepository, ICurrentUser currentUser) : ICommandHandler<CreateLeaveTypeCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var leaveType = new LeaveTypeEntity(currentUser.OrganizationId, command.Name, command.Code, command.DefaultDaysPerYear, command.IsPaid, command.RequiresApproval);
            var alreadyExist = await leaveRepository.NameOrCodeExistAsync(command.Name, command.Code, currentUser.OrganizationId, cancellationToken);

            if (alreadyExist) return Error.Conflict(description: "name or code already exist");
            
            var result = await leaveRepository.CreateLeaveTypeAsync(leaveType, cancellationToken);

            if (!result) return Error.Failure(description: "leave type is not created");

            return true;
        }
    }
}