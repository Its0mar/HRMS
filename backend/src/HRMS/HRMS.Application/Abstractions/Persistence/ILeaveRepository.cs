using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface ILeaveRepository
    {
        Task<bool> CreateLeaveTypeAsync(LeaveType leaveType, CancellationToken cancellationToken);
        Task<bool> NameOrCodeExistAsync(string name, string code, int organizationId, CancellationToken cancellationToken);
        Task<IReadOnlyList<LeaveType>> GetForOrganizationAsync(int organizationId, CancellationToken cancellationToken);
        Task<LeaveType?> GetByIdAsync(int id, int organizationId, CancellationToken cancellationToken);
        Task<bool> UpdateLeaveTypeAsync(LeaveType leaveType, CancellationToken cancellationToken);
        public Task<List<MyLeaveBalancesResponse>> GetMyBalancesAsync(int year, int employeeId, int organizationId, CancellationToken cancellationToken);
    }
}
