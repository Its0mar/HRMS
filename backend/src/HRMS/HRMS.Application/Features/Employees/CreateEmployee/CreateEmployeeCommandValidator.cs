using FluentValidation;
using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Application.Features.Employees.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeNumber)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(8);

            RuleFor(x => x.FirstName)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(15);

            RuleFor(x => x.LastName)
               .NotNull()
               .MinimumLength(3)
               .MaximumLength(15);

            RuleFor(x => x.NationalId)
               .NotNull()
               .MinimumLength(8)
               .MaximumLength(25);

            RuleFor(x => x.Nationality)
               .NotNull()
               .MinimumLength(8)
               .MaximumLength(25);

            RuleFor(x => x.Phone)
               .NotNull()
               .MinimumLength(8)
               .MaximumLength(25);

            RuleFor(x => x.Email)
               .NotNull()
               .EmailAddress()
               .MinimumLength(8)
               .MaximumLength(25);

            RuleFor(x => x.Address)
               .NotNull()
               .MinimumLength(30)
               .MaximumLength(300);

            RuleFor(x => x.ProfilePictureUrl)
               .MinimumLength(10)
               .MaximumLength(300);

        }
    }
}
