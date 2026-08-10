
using HRMS.Domain.Entities.Roles;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IRolesRepository
    {
        Task<IReadOnlyList<Role>> GetAllWithPermsAsync(int organizationId, CancellationToken cancellationToken);
        Task<int> CreateWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(int id, int organizationId, CancellationToken cancellationToken);
        Task<int> UpdateWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, CancellationToken cancellationToken);
    }
}