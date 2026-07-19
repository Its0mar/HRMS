using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IDepartmentRepository
{
    Task<int> CreateAsync(Department department, CancellationToken cancellationToken);
    Task<bool> NameExistsAsync(int organizationId, string name, CancellationToken cancellationToken);
    Task<bool> CodeExistsAsync(int organizationId, string code, CancellationToken cancellationToken);
}
