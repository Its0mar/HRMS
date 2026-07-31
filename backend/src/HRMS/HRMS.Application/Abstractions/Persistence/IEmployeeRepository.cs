using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IEmployeeRepository
    {
        public Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken);
        public Task<List<EmployeeOptionResponse>> GetEmployeesOptionsAsync(int organizationId, CancellationToken cancellationToken);

    }
}
