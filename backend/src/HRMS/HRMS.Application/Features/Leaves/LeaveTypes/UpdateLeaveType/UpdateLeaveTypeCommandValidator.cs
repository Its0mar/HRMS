using FluentValidation;

namespace HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType
{
    public sealed class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
    {
        public UpdateLeaveTypeCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(1);
            RuleFor(x => x.Name).NotEmpty().Length(2, 100);
            RuleFor(x => x.Code).NotEmpty().Length(2, 20);
            RuleFor(x => x.DefaultDaysPerYear).InclusiveBetween(0, 365);
        }
    }
}
