using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.WorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.GetWorkSchedules
{
    public class GetWorkSchedulesQueryHandler : IQueryHandler<GetWorkSchedulesQuery, List<WorkScheduleResponse>>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public GetWorkSchedulesQueryHandler(IWorkScheduleRepository workScheduleRepository, ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<List<WorkScheduleResponse>>> HandleAsync(GetWorkSchedulesQuery query, CancellationToken cancellationToken)
        {
            var workScheduleList =  await _workScheduleRepository.GetWorkSchedulesByOrganizationIdAsync(_currentUser.OrganizationId, cancellationToken);
            
            return workScheduleList.Select(x => ToWorkScheduleResponse(x)).ToList() ?? new List<WorkScheduleResponse>();
        }

        private WorkScheduleResponse ToWorkScheduleResponse(WorkSchedule workSchedule)
        {
            return new WorkScheduleResponse(
                workSchedule.Id ?? -1,
                workSchedule.Name,
                workSchedule.GracePeriodMinutes,
                workSchedule.IsDefault
            );
        }
    }
}
