using HRMS.Application.Abstractions.Persistence.Models;


namespace HRMS.Application.Abstractions.Persistence
{
    public interface IPermissionsRepository
    {
        Task<IReadOnlyList<PermissionOption>> GetAllAsync(CancellationToken cancellationToken);
    }
}
