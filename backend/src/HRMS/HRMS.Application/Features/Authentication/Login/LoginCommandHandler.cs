using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Authentication.Login
{
    internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public LoginCommandHandler(
            IUserRepository users,
            IRefreshTokenRepository refreshTokenrepository,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator tokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator)
        {
            _userRepository = users;
            _refreshTokenRepository = refreshTokenrepository; 
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = tokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<ErrorOr<LoginResponse>> HandleAsync(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var identifier = command.Identifier.Trim();

            if (identifier.Contains('@'))
            {
                identifier = identifier.ToLowerInvariant();
            }

            var user = await _userRepository.GetByIdentifierAsync(
                identifier,
                cancellationToken);

            if (user is null ||
                user.IsDeleted ||
                !user.IsActive ||
                !_passwordHasher.Verify(
                    command.Password,
                    user.PasswordHash))
            {
                return AuthenticationErrors.InvalidCredentials;
            }

            if (user.Id is not int userId)
            {
                return Error.Unexpected(
                    code: "Authentication.UserIdMissing",
                    description: "The authenticated user has no ID.");
            }

            var permissions = await _userRepository.GetUserPermissions(userId, cancellationToken);
            var accessToken = _accessTokenGenerator.Generate(user, permissions);
            var rawRefreshToken = _refreshTokenGenerator.Generate();
            var refreshTokenHash = _refreshTokenGenerator.Hash(rawRefreshToken);

            var createdAt = DateTime.UtcNow;
            var expiresAt = createdAt.AddDays(7);

            await _refreshTokenRepository.CreateOrReplaceAsync(userId, refreshTokenHash, expiresAt, createdAt, cancellationToken);
            
            return new LoginResponse(
            User: AuthenticatedUserResponse.From(user, permissions),
                AccessToken: accessToken,
                RefreshToken: rawRefreshToken,
                RefreshTokenExpiresAt: expiresAt);
        }
    }
}
