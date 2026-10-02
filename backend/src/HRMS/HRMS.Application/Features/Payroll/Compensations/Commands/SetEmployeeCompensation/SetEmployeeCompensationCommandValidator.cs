using FluentValidation;
using HRMS.Application.Features.Payroll.Compensations.Dtos;

namespace HRMS.Application.Features.Payroll.Compensations.Commands.SetEmployeeCompensation
{
    public class SetEmployeeCompensationCommandValidator : AbstractValidator<SetEmployeeCompensationCommand>
    {
        public SetEmployeeCompensationCommandValidator()
        {
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("EmployeeId must be greater than 0.");
            RuleFor(x => x.BasicSalary).GreaterThan(0).WithMessage("BasicSalary must be greater than 0.");
            RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required.");
            RuleFor(x => x.EffectiveFrom).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("EffectiveFrom cannot be in the future.");
            RuleForEach(x => x.Allowances).SetValidator(new CompensationAllowanceDtoValidator());
        }
    }

    public class CompensationAllowanceDtoValidator : AbstractValidator<CompensationAllowanceDto>
    {
        public CompensationAllowanceDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Allowance title is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Allowance amount must be greater than 0.");
        }
    }
}
