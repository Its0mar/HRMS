using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Authentication.RegisterOrganization
{
    internal sealed class RegisterOrganizationCommandHandler
    : ICommandHandler<
        RegisterOrganizationCommand,
        RegisterOrganizationResponse>
    {
        private readonly IOrganizationRegistrationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<RegisterOrganizationCommand> _validator;

        public RegisterOrganizationCommandHandler(
            IOrganizationRegistrationRepository repository,
            IPasswordHasher passwordHasher,
            IValidator<RegisterOrganizationCommand> validator)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _validator = validator;
        }

        public async Task<ErrorOr<RegisterOrganizationResponse>> HandleAsync(
            RegisterOrganizationCommand command,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(
                command,
                cancellationToken);

            if (!validation.IsValid)
            {
                return validation.Errors
                    .Select(failure => Error.Validation(
                        code: $"RegisterOrganization.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();
            }

            var organizationCode = command.OrganizationCode.Trim().ToUpperInvariant();
            var organizationEmail = command.OrganizationEmail.Trim().ToLowerInvariant();
            var ownerEmail = command.OwnerEmail.Trim().ToLowerInvariant();
            var ownerUsername = command.OwnerUsername.Trim();

            var conflict = await FindConflictAsync(
                organizationCode,
                organizationEmail,
                ownerEmail,
                ownerUsername,
                cancellationToken);

            if (conflict is not null)
            {
                return conflict.Value;
            }

            var organization = new Organization(
                name: command.OrganizationName.Trim(),
                code: organizationCode,
                email: organizationEmail,
                address: NormalizeOptional(command.Address),
                website: NormalizeOptional(command.Website),
                logoUrl: NormalizeOptional(command.LogoUrl));

            var owner = new OwnerRegistrationData(
                Username: ownerUsername,
                Email: ownerEmail,
                PasswordHash: _passwordHasher.Hash(command.Password),
                FirstName: command.FirstName.Trim(),
                LastName: command.LastName.Trim());

            var result = await _repository.RegisterAsync(
                organization,
                owner,
                cancellationToken);

            return new RegisterOrganizationResponse(
                result.OrganizationId,
                result.OwnerUserId);
        }

        private async Task<Error?> FindConflictAsync(
            string organizationCode,
            string organizationEmail,
            string ownerEmail,
            string ownerUsername,
            CancellationToken cancellationToken)
        {
            if (await _repository.OrganizationCodeExistsAsync(
                    organizationCode,
                    cancellationToken))
            {
                return AuthenticationErrors.OrganizationCodeExists;
            }

            if (await _repository.OrganizationEmailExistsAsync(
                    organizationEmail,
                    cancellationToken))
            {
                return AuthenticationErrors.OrganizationEmailExists;
            }

            if (await _repository.UserEmailExistsAsync(
                    ownerEmail,
                    cancellationToken))
            {
                return AuthenticationErrors.UserEmailExists;
            }

            if (await _repository.UsernameExistsAsync(
                    ownerUsername,
                    cancellationToken))
            {
                return AuthenticationErrors.UsernameExists;
            }

            return null;
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}


