using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Employees.GetEmployeeOptions
{
    public class GetEmployeeOptionsHandler : IQueryHandler<GetEmployeeOptionsQuery, IReadOnlyList<EmployeeOptionResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeOptionsHandler(ICurrentUser currentUser, IEmployeeRepository employeeRepository)
        {
            _currentUser = currentUser;
            _employeeRepository = employeeRepository;
        }

        public async Task<ErrorOr<IReadOnlyList<EmployeeOptionResponse>>> HandleAsync(GetEmployeeOptionsQuery query, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesOptionsAsync(_currentUser.OrganizationId, cancellationToken);

            return employees;
        }
    }
}
