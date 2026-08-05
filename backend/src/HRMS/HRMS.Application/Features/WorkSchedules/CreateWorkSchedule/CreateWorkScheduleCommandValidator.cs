using FluentValidation;
using HRMS.Application.Features.WorkSchedules.Common;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedule
{
    public sealed class CreateWorkScheduleCommandValidator : AbstractValidator<CreateWorkScheduleCommand>
    {
        public CreateWorkScheduleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                .Length(3, 30).WithMessage("Name must be between 3 and 30 characters.");

            RuleFor(x => x.GracePeriodMinutes).GreaterThanOrEqualTo(0).WithMessage("Grace period must be greater than or equal to 0.")
                .LessThan(1440).WithMessage("Grace period must less that 1440.");

            RuleFor(x => x.WorkScheduleDay)
                .NotNull()
                .Must(days => days.Count == 7)
                .WithMessage("A work schedule must contain seven days.");

            RuleFor(x => x.WorkScheduleDay)
                .Must(days =>
                    days.Select(x => x.WorkDay).Distinct().Count()
                    == days.Count)
                .WithMessage("Each work day can appear only once.");

            RuleForEach(x => x.WorkScheduleDay).SetValidator(new WorkScheduleDayDtoValidator());

        }
    }
}