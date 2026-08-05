using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.WorkSchedules;


namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedules
{
    public class CreateWorkScheduleHandler : ICommandHandler<CreateWorkScheduleCommand, int>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public CreateWorkScheduleHandler(IWorkScheduleRepository workScheduleRepository, ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }
        
        public async Task<ErrorOr<int>> HandleAsync(CreateWorkScheduleCommand command, CancellationToken cancellationToken)
        {
            if (await _workScheduleRepository.NameExistAsync(command.Name, _currentUser.OrganizationId, null, cancellationToken))
            {
                return Error.Conflict(description: "A work schedule with with name already exist in the organization");
            }

            var workDays = command.WorkScheduleDay.Select(w =>
            {
                return new WorkScheduleDay(
                    w.WorkDay,
                    w.IsWorkingDay,
                    w.StartTime,
                    w.EndTime,
                    w.MinimumMinutesPerDay,
                    w.BreakDurationMinutes
                );
            });

            var workSchedule = new WorkSchedule(
               _currentUser.OrganizationId,
               command.Name,
               command.GracePeriodMinutes,
               workDays,
               command.IsDefault
            );

            return await _workScheduleRepository.CreateWorkScheduleAsync(workSchedule, cancellationToken);
        }
    }
}
