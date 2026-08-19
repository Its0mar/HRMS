using FluentValidation;

namespace HRMS.Application.Features.Attendance.SubmitCorrection;

public sealed class SubmitCorrectionCommandValidator : AbstractValidator<SubmitCorrectionCommand>
{
    public SubmitCorrectionCommandValidator()
    {
        RuleFor(x => x.AttendanceLogId)
            .GreaterThanOrEqualTo(1)
            .When(x => x.AttendanceLogId.HasValue)
            .WithMessage("AttendanceLogId must be a valid ID when provided.");

        RuleFor(x => x.RequestedClockIn)
            .LessThan(DateTime.UtcNow)
            .WithMessage("Requested Clock-In time cannot be in the future.");

        RuleFor(x => x.RequestedClockOut)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Requested Clock-Out time cannot be in the future.")
            .GreaterThan(x => x.RequestedClockIn)
            .WithMessage("Requested Clock-Out time must be strictly after Requested Clock-In time.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("A reason for the correction request is required.")
            .MaximumLength(300)
            .WithMessage("Reason cannot exceed 300 characters.");
    }
}