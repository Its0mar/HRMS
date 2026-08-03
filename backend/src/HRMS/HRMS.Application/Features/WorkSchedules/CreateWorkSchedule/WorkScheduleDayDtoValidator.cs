using FluentValidation;
using HRMS.Application.Features.WorkSchedules.CreateWorkSchedules;

namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedule
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
                    .GreaterThan((short)-1).WithMessage("Minimum minutes per day must be greater than -1.")
                    .LessThanOrEqualTo((short)1440).WithMessage("Minimum minutes per day cannot exceed 1440 minutes (24 hours).");

                RuleFor(x => x.BreakDurationMinutes)
                    .GreaterThanOrEqualTo((short)-1).WithMessage("Break duration cannot be negative.")
                    .LessThanOrEqualTo((short)1440).WithMessage("Break duration cannot exceed 1440 minutes (24 hours).");
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
