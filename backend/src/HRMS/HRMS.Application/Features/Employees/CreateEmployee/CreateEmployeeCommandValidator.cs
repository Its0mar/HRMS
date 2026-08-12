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

            When(command => command.ProfilePicture is not null, () =>
            {
                RuleFor(command => command.ProfilePicture!.Length)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(5 * 1024 * 1024);

                RuleFor(command => command.ProfilePicture!.ContentType)
                    .Must(type => type is
                        "image/jpeg" or
                        "image/png" or
                        "image/webp")
                    .WithMessage("Only JPEG, PNG, and WebP images are allowed.");
            });
        }
    }
}
