using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IEmployeeRepository
    {
        public Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken);
    }
}
