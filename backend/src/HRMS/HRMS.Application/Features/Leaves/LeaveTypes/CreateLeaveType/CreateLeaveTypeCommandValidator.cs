using FluentValidation;

namespace HRMS.Application.Features.Leaves.LeaveTypes.CreateLeaveType
{
    public sealed class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
    {
        public CreateLeaveTypeCommandValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.Code).MinimumLength(2).MaximumLength(20);
            RuleFor(x => x.DefaultDaysPerYear).GreaterThanOrEqualTo(1).LessThanOrEqualTo(365);

        }
    }
}