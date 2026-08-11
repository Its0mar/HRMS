
using FluentValidation;

namespace HRMS.Application.Features.Employees.Access.CreateEmployeeAccess
{
    public class RegisterEmployeeCommandValidator : AbstractValidator<RegisterEmployeeCommand>
    {
        public RegisterEmployeeCommandValidator()
        { 
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(8);
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3);
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();

        }
    }
}
