using HRMS.Application.Features.Authentication.RegisterOrganization;
using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IOrganizationRegistrationRepository
{
    Task<bool> OrganizationCodeExistsAsync(
        string code,
        CancellationToken cancellationToken);

    Task<bool> OrganizationEmailExistsAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> UserEmailExistsAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> UsernameExistsAsync(
        string username,
        CancellationToken cancellationToken);

    Task<OrganizationRegistrationResult> RegisterAsync(
        Organization organization,
        OwnerRegistrationData owner,
        CancellationToken cancellationToken);
}
