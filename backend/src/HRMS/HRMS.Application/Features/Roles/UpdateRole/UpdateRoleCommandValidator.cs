using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Roles.UpdateRole
{
    public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0);

            RuleFor(command => command.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(command => command.PermissionIds)
                .NotEmpty()
                .WithMessage("Select at least one permission.");

            RuleForEach(command => command.PermissionIds)
                .GreaterThan(0);

            RuleFor(command => command.PermissionIds)
                .Must(permissionIds =>
                    permissionIds.Count ==
                    permissionIds.Distinct().Count())
                .WithMessage(
                    "Duplicate permissions are not allowed.");
        }
    }
}
