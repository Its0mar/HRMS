using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);
    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<IReadOnlyList<string>> GetUserPermissions(int userId, CancellationToken cancellationToken);
    Task<bool> UpdateAccessAsync(int employeeId, int organizationId, string username, int roleId, CancellationToken cancellationToken);
}
