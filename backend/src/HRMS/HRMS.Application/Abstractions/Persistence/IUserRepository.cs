using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);
    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
