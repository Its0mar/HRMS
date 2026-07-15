using FluentValidation;
using HRMS.Application.Authentication.Dtos;

namespace HRMS.Application.Authentication.Validators
{
    public sealed class RegisterOrganizationValidator
    : AbstractValidator<RegisterRequest>
    {
        public RegisterOrganizationValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty()
                .WithMessage("Organization name is required.")
                .Length(3, 30)
                .WithMessage("Organization name must be between 3 and 30 characters.");

            RuleFor(x => x.OrganizationCode)
                .NotEmpty()
                .WithMessage("Organization code is required.")
                .Length(3, 10)
                .WithMessage("Organization code must be between 3 and 10 characters.");

            RuleFor(x => x.OrganizationEmail)
                .NotEmpty()
                .WithMessage("Organization email is required.")
                .MaximumLength(40)
                .WithMessage("Organization email cannot exceed 40 characters.")
                .EmailAddress()
                .WithMessage("Organization email must be valid.");

            RuleFor(x => x.Address)
                .Length(3, 100)
                .When(x => !string.IsNullOrWhiteSpace(x.Address));

            RuleFor(x => x.Website)
                .MaximumLength(100)
                .Must(value =>
                    value is null ||
                    Uri.TryCreate(value, UriKind.Absolute, out _))
                .WithMessage("Website must be a valid absolute URL.");

            RuleFor(x => x.LogoUrl)
                .MaximumLength(100)
                .Must(value =>
                    value is null ||
                    Uri.TryCreate(value, UriKind.Absolute, out _))
                .WithMessage("Logo URL must be a valid absolute URL.");

            RuleFor(x => x.OwnerUsername)
                .NotEmpty()
                .Length(3, 20);

            RuleFor(x => x.OwnerEmail)
                .NotEmpty()
                .MaximumLength(40)
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 20);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 20);
        }
    }
}
