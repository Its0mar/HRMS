using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.WorkSchedules.GetWorkScheduleOptions
{
    public sealed class GetWorkScheduleOptionsHandler : IQueryHandler<GetWorkScheduleOptionsQuery, IReadOnlyList<GetWorkScheduleOptionsResponse>>
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ICurrentUser _currentUser;

        public GetWorkScheduleOptionsHandler(IWorkScheduleRepository workScheduleRepository, ICurrentUser currentUser)
        {
            _workScheduleRepository = workScheduleRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<IReadOnlyList<GetWorkScheduleOptionsResponse>>> HandleAsync(GetWorkScheduleOptionsQuery query, CancellationToken cancellationToken)
        {
            var workSchedules = await _workScheduleRepository.GetWorkSchedulesByOrganizationIdAsync(_currentUser.OrganizationId, cancellationToken);

            return workSchedules.Select(ws => new GetWorkScheduleOptionsResponse(ws.Id ?? 0, ws.Name)).ToList();
        }

    }
}
