
using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.WorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.GetWorkScheduleWithDays
{
    public sealed class GetWorkScheduleWithDaysHandler
        : IQueryHandler<GetWorkScheduleWithDaysQuery, WorkScheduleWithDaysResponse>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public GetWorkScheduleWithDaysHandler(IWorkScheduleRepository workScheduleRepository, ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<WorkScheduleWithDaysResponse>> HandleAsync(GetWorkScheduleWithDaysQuery query, CancellationToken cancellationToken)
        {
            var workScheduleWithDays = await _workScheduleRepository.GetWorkScheduleByIdAsync(query.Id, _currentUser.OrganizationId, cancellationToken);
            if (workScheduleWithDays is null) 
                return Error.NotFound("Work Schedule with provided id int not found");

            return ToWorkSchedule(workScheduleWithDays);

        }

        private WorkScheduleDayResponseDto ToDayDto(WorkScheduleDay day)
        {
            return new WorkScheduleDayResponseDto(day.WorkDay, day.IsWorkingDay, day.StartTime, day.EndTime, day.MinimumMinutesPerDay, (short)day.BreakDurationMinutes);
        }

        private WorkScheduleWithDaysResponse ToWorkSchedule(WorkSchedule workSchedule)
        {
            var daysDto = workSchedule.Days.Select(day => (ToDayDto(day))).ToList();

            return new WorkScheduleWithDaysResponse(
                workSchedule.Id ?? 0,
                workSchedule.Name,
                workSchedule.GracePeriodMinutes,
                workSchedule.IsDefault,
                workSchedule.IsActive,
                daysDto);
        }
    }
}