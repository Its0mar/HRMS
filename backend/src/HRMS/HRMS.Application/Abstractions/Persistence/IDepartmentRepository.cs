using HRMS.Application.Features.Departments.GetDepartments;
using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IDepartmentRepository
{
    Task<int> CreateAsync(Department department, CancellationToken cancellationToken);
    Task<Department?> GettByIdAsync(int id, int organizationId, CancellationToken ct);
    Task<bool> UpdateDepartmentAsync(int departmentId, Department department, CancellationToken cancellationToken);
    public Task<List<DepartmentListItem>> GetDepartmentsAsync(int organizationId, CancellationToken cancellationToken);



    Task<bool> NameExistsAsync(int organizationId, string name, CancellationToken cancellationToken);
    Task<bool> CodeExistsAsync(int organizationId, string code, CancellationToken cancellationToken);

    
}
