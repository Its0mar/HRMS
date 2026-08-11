using FluentValidation;

namespace HRMS.Application.Features.Organizations.Registration
{
    public sealed class RegisterOrganizationCommandValidator
    : AbstractValidator<RegisterOrganizationCommand>
    {
        public RegisterOrganizationCommandValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty()
                .Length(3, 30);

            RuleFor(x => x.OrganizationCode)
                .NotEmpty()
                .Length(3, 10)
                .Matches("^[a-zA-Z0-9_-]+$");

            RuleFor(x => x.OrganizationEmail)
                .NotEmpty()
                .MaximumLength(40)
                .EmailAddress();

            RuleFor(x => x.Address)
                .Length(3, 100)
                .When(x => !string.IsNullOrWhiteSpace(x.Address));

            RuleFor(x => x.Website)
                .MaximumLength(100)
                .Must(BeValidOptionalUrl)
                .WithMessage("Website must be a valid absolute URL.");

            RuleFor(x => x.LogoUrl)
                .MaximumLength(100)
                .Must(BeValidOptionalUrl)
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

        private static bool BeValidOptionalUrl(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ||
                   Uri.TryCreate(value, UriKind.Absolute, out _);
        }
    }
}
