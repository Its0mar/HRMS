using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Authentication;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Organizations.Registration
{
    internal sealed class RegisterOrganizationCommandHandler(
        IRegistrationRepository registrationRepository,
        IPasswordHasher passwordHasher) : ICommandHandler<RegisterOrganizationCommand, RegisterOrganizationResponse>
    {
        public async Task<ErrorOr<RegisterOrganizationResponse>> HandleAsync(RegisterOrganizationCommand command, CancellationToken cancellationToken)
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

            var passwordHash =  passwordHasher.Hash(command.Password);

            var user = new User(
                ownerUsername,
                ownerEmail,
                passwordHash,
                command.FirstName.Trim(),
                command.LastName.Trim(),
                -1); 

            var result = await registrationRepository.RegisterOrganizationWithUserAsync(
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
            if (await registrationRepository.OrganizationCodeExistsAsync(
                    organizationCode,
                    cancellationToken))
            {
                return AuthenticationErrors.OrganizationCodeExists;
            }

            if (await registrationRepository.OrganizationEmailExistsAsync(
                    organizationEmail,
                    cancellationToken))
            {
                return AuthenticationErrors.OrganizationEmailExists;
            }

            if (await registrationRepository.UserEmailExistsAsync(
                    ownerEmail,
                    cancellationToken))
            {
                return AuthenticationErrors.UserEmailExists;
            }

            if (await registrationRepository.UsernameExistsAsync(
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


