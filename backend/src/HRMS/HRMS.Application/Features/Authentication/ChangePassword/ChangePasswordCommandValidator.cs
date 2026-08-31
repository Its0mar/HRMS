using FluentValidation;

namespace HRMS.Application.Features.Authentication.ChangePassword
{
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MinimumLength(8).WithMessage("Current password must be at least 8 characters.")
                .MaximumLength(100);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters.")
                .MaximumLength(100)
                .NotEqual(x => x.OldPassword).WithMessage("New password must be different from current password.");
        }
    }
}
