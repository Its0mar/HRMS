using HRMS.Domain.Entities.WorkSchedules;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IWorkScheduleRepository
    {
        public Task<int> CreateWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken cancellationToken);
    }
}
