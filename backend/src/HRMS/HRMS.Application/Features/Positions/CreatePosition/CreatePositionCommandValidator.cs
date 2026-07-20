using FluentValidation;

namespace HRMS.Application.Features.Positions.CreatePosition
{
    public class CreatePositionCommandValidator
        : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionCommandValidator()
        {
            RuleFor(x => x.Title).Length(3, 20).NotEmpty();
            RuleFor(x => x.Description).Length(3, 300);
        }
    }
}
