using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Services;
using HRMS.Domain.Entities.Employees;

namespace HRMS.Application.Features.Employees.CreateEmployee
{
    public class CreateEmployeeHandler
        : ICommandHandler<CreateEmployeeCommand, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IFileService _fileService;

        public CreateEmployeeHandler(
            IEmployeeRepository employeeRepository,
            ICurrentUser currentUser,
            IFileService fileServcie)
        {
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _fileService = fileServcie;
        }

        public async Task<ErrorOr<CreateEmployeeResponse>> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var personalInformation = CreatePersonalInformationFromCommand(command);
            var employmentInformation = CreateEmploymentInformationFromCommand(command);

            var employee = new Employee(
                command.EmployeeNumber,
                _currentUser.OrganizationId,
                personalInformation,
                employmentInformation);

            var result =  await _employeeRepository.CreateAsync(employee, cancellationToken);

            if (command.ProfilePicture is not null)
            {
                var path = await _fileService.UploadFileAsync(
                    command.ProfilePicture.Content,
                    command.ProfilePicture.FileName,
                    "employees-profile-pic",
                    true,
                    cancellationToken);
            }

            return new CreateEmployeeResponse(result);

        }

        private PersonalInformation CreatePersonalInformationFromCommand(CreateEmployeeCommand command)
        {
            return new PersonalInformation(
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
                null);
        }
        private EmploymentInformation CreateEmploymentInformationFromCommand(CreateEmployeeCommand command)
        {
            return new EmploymentInformation(
                command.DepartmentId,
                command.PositionId,
                command.ManagerEmployeeId,
                command.HireDate,
                command.EmploymentType,
                command.EmploymentStatus,
                command.WorkEmail,
                command.WorkPhone);
        }

    }
}
