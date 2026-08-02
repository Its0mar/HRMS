using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Features.Employees.GetEmployees
{
    public class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, IReadOnlyList<GetEmployeesResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesQueryHandler(ICurrentUser currentUser, IEmployeeRepository employeeRepository)
        {
            _currentUser = currentUser;
            _employeeRepository = employeeRepository;
        }

        public async Task<ErrorOr<IReadOnlyList<GetEmployeesResponse>>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesAsync(_currentUser.OrganizationId, cancellationToken);

            return employees;
        }


    }
}
