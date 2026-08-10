using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence;

public interface IRegistrationRepository
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

    Task<OrganizationRegistrationResult> RegisterOrganizationWithUserAsync(
        Organization organization,
        User user,
        CancellationToken cancellationToken);

    public Task<int> UserRegisterAsync(
        User user,
        int roleId,
        CancellationToken cancellationToken);
}
