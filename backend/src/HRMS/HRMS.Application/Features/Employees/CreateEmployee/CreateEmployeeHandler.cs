using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Features.Employees.CreateEmployee
{
    public class CreateEmployeeHandler
        : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUser _currentUser;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, ICurrentUser currentUser)
        {
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<CreateEmployeeResponse>> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var personalInformation = new PersonalInformation(
                command.FirstName,
                command.LastName,
                command.DateOfBirth,
                command.Gender,
                command.NationalId,
                command.Nationality,
                command.MaritalStatus,
                command.Phone,
                command.Email,
                command.Address,
                command.ProfilePictureUrl);

            var employmentInformation = new EmploymentInformation(
                command.DepartmentId,
                command.PositionId,
                command.ManagerEmployeeId,
                command.HireDate,
                command.EmploymentType,
                command.EmploymentStatus,
                command.WorkEmail,
                command.WorkPhone);

            var employee = new Employee(
                "emp-1",
                _currentUser.OrganizationId,
                personalInformation,
                employmentInformation);

            var result =  await _employeeRepository.CreateAsync(employee, cancellationToken);

            return new CreateEmployeeResponse(result);

        }
    }
}
