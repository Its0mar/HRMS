
using HRMS.Domain.Entities.Roles;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IRolesRepository
    {
        Task<IReadOnlyList<Role>> GetAllAsync(int organizationId, CancellationToken cancellationToken);
    }
}
