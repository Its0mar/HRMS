using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Departments.CreateDepartment
{
    internal sealed class CreateDepartmentCommandHandler
    : ICommandHandler<
        CreateDepartmentCommand,
        CreateDepartmentResponse>
    {
        private readonly IDepartmentRepository _departments;
        private readonly ICurrentUser _currentUser;
        private readonly IValidator<CreateDepartmentCommand> _validator;

        public CreateDepartmentCommandHandler(
            IDepartmentRepository departments,
            ICurrentUser currentUser,
            IValidator<CreateDepartmentCommand> validator)
        {
            _departments = departments;
            _currentUser = currentUser;
            _validator = validator;
        }

        public async Task<ErrorOr<CreateDepartmentResponse>> HandleAsync(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(
                command,
                cancellationToken);

            if (!validation.IsValid)
            {
                return validation.Errors
                    .Select(failure => Error.Validation(
                        code: $"CreateDepartment.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();
            }

            var organizationId = _currentUser.OrganizationId;
            var name = command.Name.Trim();
            var code = command.Code.Trim().ToUpperInvariant();

            if (await _departments.NameExistsAsync(
                    organizationId,
                    name,
                    cancellationToken))
            {
                return DepartmentErrors.NameExists;
            }

            if (await _departments.CodeExistsAsync(
                    organizationId,
                    code,
                    cancellationToken))
            {
                return DepartmentErrors.CodeExists;
            }

            var department = new Department(
                name,
                code,
                organizationId,
                NormalizeOptional(command.Description),
                command.ManagerId);

            var departmentId = await _departments.CreateAsync(
                department,
                cancellationToken);

            if (departmentId <= 0)
            {
                return DepartmentErrors.CreationFailed;
            }

            return new CreateDepartmentResponse(departmentId);
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}
