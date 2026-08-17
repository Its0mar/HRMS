using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;


namespace HRMS.Application.Features.Authentication.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator) : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        public async Task<ErrorOr<RefreshTokenResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var currentTokenHash = refreshTokenGenerator.Hash(command.refreshToken);
            var currentToken = await refreshTokenRepository.GetByHashAsync(currentTokenHash, cancellationToken);

            if (currentToken is null || !currentToken.IsValid)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var user = await userRepository.GetByIdAsync(currentToken.UserId, cancellationToken);

            if (user is null || !user.CanAuthenticate)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var newRawToken = refreshTokenGenerator.Generate();
            var newTokenHash = refreshTokenGenerator.Hash(newRawToken);

            var expiresAt = DateTime.UtcNow.AddDays(7);
            var createdAt = DateTime.UtcNow;

            var rotated = await refreshTokenRepository.RotateAsync(
                user.Id!.Value,
                currentTokenHash,
                newTokenHash,
                expiresAt,
                createdAt,
                cancellationToken);

            if (!rotated)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var permissions = await userRepository.GetUserPermissions(user.Id.Value, cancellationToken);

            var accessToken = accessTokenGenerator.Generate(user, permissions);

            return new RefreshTokenResponse(
                AccessToken: accessToken,
                RefreshToken: newRawToken,
                RefreshTokenExpiresAt: expiresAt);
        }
    }
}
