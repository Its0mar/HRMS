using ErrorOr;
using HRMS.Application.Authentication.Dtos;
using HRMS.Application.Authentication.Interfaces;
using HRMS.Domain.Entities;

namespace HRMS.Application.Authentication.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IRegistrationRepository registrationRepository,
            IPasswordHasher passwordHasher)
        {
            _registrationRepository = registrationRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ErrorOr<RegisterResponse>> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {

            var userEmail = request.OwnerEmail.Trim().ToLowerInvariant();
            var username = request.OwnerUsername.Trim();
            var passwordHash = _passwordHasher.Hash(request.Password);

            var organization = CreateOrganizationObject(request);

            var isExist = await CheckIfExistAsync(organization.Code, organization.Email, userEmail, username, cancellationToken);

            if (isExist.IsError)
            {
                return isExist.Errors;
            }
            

            //User CreateOwner(int organizationId)
            //{
            //    return new User(
            //        username: username,
            //        email: userEmail,
            //        passwordHash: passwordHash,
            //        firstName: request.FirstName.Trim(),
            //        lastName: request.LastName.Trim(),
            //        organizationId: organizationId);
            //}

            var result = await _registrationRepository.RegisterAsync(
                organization,
                GetCreateOwnerFunc(request, userEmail, username, passwordHash),
                cancellationToken);

            return new RegisterResponse(
                OrganizationId: result.OrganizationId,
                UserId: result.UserId);
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private async Task<ErrorOr<bool>> CheckIfExistAsync(string organizationCode, string organizationEmail, string userEmail, string username, CancellationToken cancellationToken)
        {
            if (await _registrationRepository.OrganizationCodeExistsAsync(organizationCode, cancellationToken))
            {
                return AuthErrors.OrganizationCodeExists;
            }
            if (await _registrationRepository.OrganizationEmailExistsAsync(organizationEmail, cancellationToken))
            {
                return AuthErrors.OrganizationEmailExists;
            }
            if (await _registrationRepository.UserEmailExistsAsync(userEmail, cancellationToken))
            {
                return AuthErrors.UserEmailExists;
            }
            if (await _registrationRepository.UsernameExistsAsync(username, cancellationToken))
            {
                return AuthErrors.UsernameExists;
            }
            return true ;
        }
    
        private Organization CreateOrganizationObject(RegisterRequest request)
        {
            var organizationCode = request.OrganizationCode.Trim().ToUpperInvariant();
            var organizationEmail = request.OrganizationEmail.Trim().ToLowerInvariant();
            var userEmail = request.OwnerEmail.Trim().ToLowerInvariant();
            var username = request.OwnerUsername.Trim();


            var organization = new Organization(
                name: request.OrganizationName.Trim(),
                code: organizationCode,
                email: organizationEmail,
                address: NormalizeOptional(request.Address),
                website: NormalizeOptional(request.Website),
                logoUrl: NormalizeOptional(request.LogoUrl));

            return organization;
        }

        private Func<int, User> GetCreateOwnerFunc(RegisterRequest request, string userEmail, string username, string passwordHash)
        {
            Func<int, User> CreateOwner = (int organizationId) =>
            {
                return new User(
                    username: username,
                    email: userEmail,
                    passwordHash: passwordHash,
                    firstName: request.FirstName.Trim(),
                    lastName: request.LastName.Trim(),
                    organizationId: organizationId);
            };

            return CreateOwner;
        }



    }
}
