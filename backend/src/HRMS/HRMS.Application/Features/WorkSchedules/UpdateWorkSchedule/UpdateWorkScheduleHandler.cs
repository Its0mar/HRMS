using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.WorkSchedules.Common;
using HRMS.Domain.Entities.WorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule
{
    public sealed class UpdateWorkScheduleHandler : ICommandHandler<UpdateWorkScheduleCommand, bool>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateWorkScheduleHandler(IWorkScheduleRepository workScheduleRepository, ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(UpdateWorkScheduleCommand command, CancellationToken cancellationToken)
        {
            if (await _workScheduleRepository.NameExistAsync(command.Name, _currentUser.OrganizationId, command.Id, cancellationToken))
            {
                return Error.Conflict(description : "Work schedule name already exists.");
            }

            var workSchedule = await _workScheduleRepository.GetWorkScheduleByIdAsync(command.Id, _currentUser.OrganizationId, cancellationToken);

            if (workSchedule is null)
            {
                return Error.NotFound(description: "Work schedule not found.");
            }

            var workScheduleDays = command.WorkScheduleDays.Select(day => day.ToWorkScheduleDay()).ToList();
            workSchedule.UpdateWorkSchedule(command.Name, command.GracePeriodMinutes, command.IsDefault, workScheduleDays);

            var result = await _workScheduleRepository.UpdateWorkScheduleAsync(workSchedule, cancellationToken);
            
            if (result > 0)
            {
                return true;
            }

            return Error.Failure(description: "Failed to update work schedule.");

        }
    }
}
