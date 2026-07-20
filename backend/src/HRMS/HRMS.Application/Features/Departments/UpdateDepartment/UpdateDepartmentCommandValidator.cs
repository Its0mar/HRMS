using FluentValidation;

namespace HRMS.Application.Features.Departments.UpdateDepartment
{
    public sealed class UpdateDepartmentCommandValidator
        : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 30);

            RuleFor(x => x.Description)
                .MaximumLength(300);

            RuleFor(x => x.ManagerEmployeeId).GreaterThan(0);

            //TODO : One of these three must be not null
        }
    }
}
