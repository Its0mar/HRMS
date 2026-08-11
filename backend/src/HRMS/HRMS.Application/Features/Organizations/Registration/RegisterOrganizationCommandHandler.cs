using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Authentication;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Organizations.Registration
{
    internal sealed class RegisterOrganizationCommandHandler
    : ICommandHandler<
        RegisterOrganizationCommand,
        RegisterOrganizationResponse>
    {
        private readonly IRegistrationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterOrganizationCommandHandler(
            IRegistrationRepository repository,
            IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ErrorOr<RegisterOrganizationResponse>> HandleAsync(
            RegisterOrganizationCommand command,
            CancellationToken cancellationToken)
        {
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

            var passwordHash =  _passwordHasher.Hash(command.Password);

            var user = new User(
                ownerUsername,
                ownerEmail,
                passwordHash,
                command.FirstName.Trim(),
                command.LastName.Trim(),
                -1); 

            //var owner = new OwnerRegistrationData(
            //    Username: ownerUsername,
            //    Email: ownerEmail,
            //    PasswordHash: _passwordHasher.Hash(command.Password),
            //    FirstName: command.FirstName.Trim(),
            //    LastName: command.LastName.Trim());

            var result = await _repository.RegisterOrganizationWithUserAsync(
                organization,
                user,
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


