using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;


namespace HRMS.Application.Features.Authentication.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IRefreshTokenGenerator refreshTokenGenerator,
            IAccessTokenGenerator accessTokenGenerator
            )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<ErrorOr<RefreshTokenResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var currentTokenHash = _refreshTokenGenerator.Hash(command.refreshToken);
            var currentToken = await _refreshTokenRepository.GetByHashAsync(currentTokenHash, cancellationToken);

            if (currentToken is null || !currentToken.IsValid)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var user = await _userRepository.GetByIdAsync(currentToken.UserId, cancellationToken);

            if (user is null || user.IsDeleted || !user.IsActive)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var newRawToken = _refreshTokenGenerator.Generate();
            var newTokenHash = _refreshTokenGenerator.Hash(newRawToken);

            var expiresAt = DateTime.UtcNow.AddDays(7);
            var createdAt = DateTime.UtcNow;

            var rotated = await _refreshTokenRepository.RotateAsync(
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

            var permissions = await _userRepository.GetUserPermissions(user.Id.Value, cancellationToken);

            var accessToken = _accessTokenGenerator.Generate(user, permissions);

            return new RefreshTokenResponse(
                AccessToken: accessToken,
                RefreshToken: newRawToken,
                RefreshTokenExpiresAt: expiresAt);
        }
    }
}
