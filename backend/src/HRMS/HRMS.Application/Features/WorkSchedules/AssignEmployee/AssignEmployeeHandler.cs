using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.WorkSchedules.AssignEmployee
{
    public sealed class AssignEmployeeHandler
        : ICommandHandler<AssignEmployeeCommand, bool>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public AssignEmployeeHandler(
            IWorkScheduleRepository workScheduleRepository,
            ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(AssignEmployeeCommand command, CancellationToken cancellationToken)
        {
            var result = await _workScheduleRepository.AssignEmployeeAsync(
                command.EmployeeId,
                command.WorkScheduleId,
                DateTime.UtcNow,
                cancellationToken);

            if (!result) return Error.Failure(description: "Assign failed");
            return true;
        }
    }
}
