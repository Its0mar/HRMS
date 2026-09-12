using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Authentication.Login
{
    internal sealed class LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ILogger<LoginCommandHandler> logger)
    : ICommandHandler<LoginCommand, LoginResponse>
    {
        public async Task<ErrorOr<LoginResponse>> HandleAsync(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var identifier = command.Identifier.Trim();

            if (identifier.Contains('@'))
            {
                identifier = identifier.ToLowerInvariant();
            }

            var user = await userRepository.GetByIdentifierAsync(identifier, cancellationToken);

            if (user is null || user.Id is not int userId || !user.CanAuthenticate || !passwordHasher.Verify(command.Password, user.PasswordHash))
            {
                logger.LogWarning("Failed authentication attempt for identifier {Identifier}", identifier);
                return AuthenticationErrors.InvalidCredentials;
            }

            var permissions = await userRepository.GetUserPermissions(userId, cancellationToken);
            var accessToken = accessTokenGenerator.Generate(user, permissions);
            var rawRefreshToken = refreshTokenGenerator.Generate();
            var refreshTokenHash = refreshTokenGenerator.Hash(rawRefreshToken);

            var createdAt = DateTime.UtcNow;
            var expiresAt = createdAt.AddDays(7);

            await refreshTokenRepository.CreateOrReplaceAsync(userId, refreshTokenHash, expiresAt, createdAt, cancellationToken);
            
            logger.LogInformation("User {UserId} ({Email}) logged in successfully", userId, user.Email);

            return new LoginResponse(
                User: AuthenticatedUserResponse.From(user, permissions),
                AccessToken: accessToken,
                RefreshToken: rawRefreshToken,
                RefreshTokenExpiresAt: expiresAt
                );
        }
    }
}
