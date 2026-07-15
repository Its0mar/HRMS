using HRMS.Application.Authentication.Dtos;
using HRMS.Domain.Entities;


namespace HRMS.Application.Authentication.Interfaces
{
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

        Task<RegisterResponse> RegisterAsync(
            Organization organization,
            Func<int, User> createOwner,
            CancellationToken cancellationToken);
    }
}
