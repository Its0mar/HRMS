using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using System.Net.Mail;

namespace HRMS.Application.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler
        : ICommandHandler<UpdateDepartmentCommand, bool>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateDepartmentCommandHandler(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            ICurrentUser currentUser)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _currentUser  = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GettByIdAsync(command.Id, _currentUser.OrganizationId, cancellationToken);
            if (department is null || department.IsDeleted || !department.IsActive)
            {
                return DepartmentErrors.NotFound;
            }

            var name = NormalizeOptional(command.Name);
            var description = NormalizeOptional(command.Description);

            if (command.Name != null)
            {
                var isNameExist = await _departmentRepository.NameExistsAsync(department.OrganizationId, command.Name, cancellationToken);
                if (isNameExist)
                {
                    return DepartmentErrors.NameExists;
                }
            }

            if (command.ManagerEmployeeId != null)
            {
                var manager = await _userRepository.GetByIdAsync(command.ManagerEmployeeId.Value, cancellationToken);
                if (manager is null || manager.OrganizationId != department.OrganizationId || manager.IsDeleted || !manager.IsActive)
                {
                    //TODO : return UserError
                    return Error.NotFound("manager not found");
                }
            }

            department.Update(name, description, command.ManagerEmployeeId);
            var result = await _departmentRepository.UpdateDepartmentAsync(command.Id, department, cancellationToken);

            if (!result) return Error.Failure();
            return true;

        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}
