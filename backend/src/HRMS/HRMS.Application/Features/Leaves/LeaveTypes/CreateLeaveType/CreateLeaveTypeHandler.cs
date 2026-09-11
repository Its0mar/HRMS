using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Services;
using LeaveTypeEntity = HRMS.Domain.Entities.Leaves.LeaveType;

namespace HRMS.Application.Features.Leaves.LeaveTypes.CreateLeaveType
{
    public sealed class CreateLeaveTypeHandler(ILeaveRepository leaveRepository) : ICommandHandler<CreateLeaveTypeCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var leaveType = new LeaveTypeEntity(command.OrganizationId, command.Name, command.Code, command.DefaultDaysPerYear, command.IsPaid, command.RequiresApproval);
            var alreadyExist = await leaveRepository.NameOrCodeExistAsync(command.Name, command.Code, command.OrganizationId, cancellationToken);

            if (alreadyExist) return Error.Conflict(description: "name or code already exist");
            
            var result = await leaveRepository.CreateLeaveTypeAsync(leaveType, cancellationToken);

            if (!result) return Error.Failure(description: "leave type is not created");

            return true;
        }
    }
}