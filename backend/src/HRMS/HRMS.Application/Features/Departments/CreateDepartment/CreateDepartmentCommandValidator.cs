using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Departments.CreateDepartment
{
    public sealed class CreateDepartmentCommandValidator
    : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 30);

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(2, 6)
                .Matches("^[a-zA-Z0-9_-]+$");

            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
