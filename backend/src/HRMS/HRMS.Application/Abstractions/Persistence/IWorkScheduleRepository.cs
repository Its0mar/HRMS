using HRMS.Domain.Entities;
using HRMS.Domain.Entities.WorkSchedules;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IWorkScheduleRepository
    {
        public Task<int> CreateWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken cancellationToken);
        public Task<int> UpdateWorkScheduleAsync(WorkSchedule workSchedule, CancellationToken cancellationToken);
        public Task<WorkSchedule?> GetWorkScheduleByIdAsync(int id, int organizationId, CancellationToken cancellationToken);
        public Task<IEnumerable<WorkSchedule>> GetWorkSchedulesByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken);
        public Task<bool> NameExistAsync(string name, int organizationId, int? excludeId, CancellationToken cancellationToken);
        public Task<bool> AssignEmployeeAsync(int employeeId, int workScheduleId, DateTime effectiveFrom, CancellationToken cancellationToken);


    }
}
