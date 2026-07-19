using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Authentication.Login
{
    public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
