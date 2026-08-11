using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Employees.Access.CreateEmployeeAccess
{
    public sealed class RegisterEmployeeHandler : ICommandHandler<RegisterEmployeeCommand, int>
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRolesRepository _rolesRepository;

        public RegisterEmployeeHandler(
            IRegistrationRepository registrationRepository,
            IEmployeeRepository employeeRepository,
            ICurrentUser currentUser,
            IPasswordHasher passwordHasher,
            IRolesRepository rolesRepository)
        {
            _registrationRepository = registrationRepository;
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _passwordHasher = passwordHasher;
            _rolesRepository = rolesRepository;
        }

        public async Task<ErrorOr<int>> HandleAsync(RegisterEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employeeInfo = await _employeeRepository.GetEmployeeInfoForUserRegisterationAsync(command.EmployeeId, _currentUser.OrganizationId, cancellationToken);

            if (employeeInfo is null)
            {
                return Error.NotFound(description: "Employee not found");
            }

            var role = await _rolesRepository.GetByIdAsync(
                command.RoleId,
                _currentUser.OrganizationId,
                cancellationToken);

            if (role is null)
            {
                return Error.NotFound(description: "The selected role was not found.");
            }

            if (await _registrationRepository.UsernameExistsAsync(command.UserName, cancellationToken))
            {
                return Error.Conflict(description: "A user with the same user name already exist");
            }

            if (await _registrationRepository.UserEmailExistsAsync(employeeInfo.Email, cancellationToken))
            {
                return Error.Conflict(description: "A user with the same email already exist");
            }

            var user = new User(
                command.UserName,
                employeeInfo.Email,
                _passwordHasher.Hash(command.Password),
                employeeInfo.FirstName,
                employeeInfo.LastName,
                _currentUser.OrganizationId,
                command.EmployeeId);

            return await _registrationRepository.UserRegisterAsync(
                user,
                command.RoleId,
                cancellationToken
                );

        }
    }
}
