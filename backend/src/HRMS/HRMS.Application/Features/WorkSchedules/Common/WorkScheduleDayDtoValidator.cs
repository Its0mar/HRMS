using FluentValidation;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.Common
{
    public sealed class WorkScheduleDayDtoValidator : AbstractValidator<WorkScheduleDayDto>
    {
        public WorkScheduleDayDtoValidator()
        {
            RuleFor(x => x.WorkDay).IsInEnum().WithMessage("not a valid day");
            
            When(x => x.IsWorkingDay, () =>
            {
                RuleFor(x => x.StartTime)
                    .NotNull().WithMessage("Start time is required for working days.");

                RuleFor(x => x.EndTime)
                    .NotNull().WithMessage("End time is required for working days.");

                RuleFor(x => x)
                    .Must(HaveValidTimeRange).WithMessage("Start time must be before end time.")
                    .Must(HaveValidBreakDuration).WithMessage("Break duration cannot exceed the scheduled work duration.");

                RuleFor(x => x.MinimumMinutesPerDay)
                    .GreaterThanOrEqualTo((short)0).WithMessage("Minimum minutes per day cannot be negative.")
                    .LessThanOrEqualTo((short)1440).WithMessage("Minimum minutes per day cannot exceed 1440 minutes (24 hours).");

                RuleFor(x => x.BreakDurationMinutes)
                    .GreaterThanOrEqualTo((short)0).WithMessage("Break duration cannot be negative.")
                    .LessThanOrEqualTo((short)1440).WithMessage("Break duration cannot exceed 1440 minutes (24 hours).");
            });

            When(x => !x.IsWorkingDay, () =>
            {
                RuleFor(x => x.StartTime).Null();
                RuleFor(x => x.EndTime).Null();

                RuleFor(x => x.MinimumMinutesPerDay)
                    .Null();

                RuleFor(x => x.BreakDurationMinutes)
                    .Equal((short)0);
            });
        }

        private static bool HaveValidTimeRange(WorkScheduleDayDto detail)
        {
            if (!detail.StartTime.HasValue || !detail.EndTime.HasValue)
            {
                return true;
            }

            return detail.StartTime.Value < detail.EndTime.Value;
        }

        private static bool HaveValidBreakDuration(WorkScheduleDayDto detail)
        {
            if (!detail.StartTime.HasValue || !detail.EndTime.HasValue)
            {
                return true;
            }

            var scheduledDuration = detail.EndTime.Value - detail.StartTime.Value;

            return detail.BreakDurationMinutes <= scheduledDuration.TotalMinutes;
        }
    }
}
