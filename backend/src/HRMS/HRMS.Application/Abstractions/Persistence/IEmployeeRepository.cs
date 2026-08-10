using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IEmployeeRepository
    {
        public Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken);
        public Task<List<GetEmployeesResponse>> GetEmployeesAsync(int organizationId, CancellationToken cancellationToken);
        public Task<List<EmployeeOptionResponse>> GetEmployeesOptionsAsync(int organizationId, CancellationToken cancellationToken);
        public Task<EmployeeInfoForUserRegister?> GetEmployeeInfoForUserRegisterationAsync(int employeeId, int organizationId, CancellationToken cancellationToken);
        Task<EmployeeAccessInfo?> GetAccessByEmployeeIdAsync(int employeeId, int organizationId, CancellationToken cancellationToken);

    }
}
