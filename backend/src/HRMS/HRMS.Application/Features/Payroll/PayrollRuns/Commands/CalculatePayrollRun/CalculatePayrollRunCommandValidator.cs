using FluentValidation;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun
{
    public class CalculatePayrollRunCommandValidator : AbstractValidator<CalculatePayrollRunCommand>
    {
        public CalculatePayrollRunCommandValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(2020).WithMessage("Year must be valid.");
            When(x => x.PayFrequency == PayFrequency.Monthly, () =>
            {
                RuleFor(x => x.Month)
                    .NotNull().WithMessage("Month is required for monthly payroll.")
                    .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");
                RuleFor(x => x.Week)
                    .Null().WithMessage("Week must be null for monthly payroll.");
            });
            When(x => x.PayFrequency == PayFrequency.Weekly, () =>
            {
                RuleFor(x => x.Week)
                    .NotNull().WithMessage("Week is required for weekly payroll.")
                    .InclusiveBetween(1, 53).WithMessage("Week must be between 1 and 53.");
                RuleFor(x => x.Month)
                    .Null().WithMessage("Month must be null for weekly payroll.");
            });
        }
    }
}
