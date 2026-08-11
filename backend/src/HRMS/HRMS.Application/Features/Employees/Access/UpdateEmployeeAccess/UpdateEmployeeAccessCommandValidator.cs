using FluentValidation;

namespace HRMS.Application.Features.Employees.UpdateEmployeeAccess
{
    public sealed class UpdateEmployeeAccessCommandValidator
    : AbstractValidator<UpdateEmployeeAccessCommand>
    {
        public UpdateEmployeeAccessCommandValidator()
        {
            RuleFor(command => command.EmployeeId)
                .GreaterThan(0);

            RuleFor(command => command.Username)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20);

            RuleFor(command => command.RoleId)
                .GreaterThan(0);
        }
    }
}
