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
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(
                command.refreshToken,
                cancellationToken);

            if (refreshToken is null || !refreshToken.IsValid)
            {
                return Error.Failure("invalid refreh token");
            }

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
            if (user is null)
            {
                return Error.NotFound("User if not found");
            }

            var userId = user.Id ?? throw new ArgumentNullException("userId can`t be null");

            var newRefreshToken = _refreshTokenGenerator.Generate();
            await _refreshTokenRepository.UpdateUserRefreshTokenAsync(refreshToken.UserId, newRefreshToken, DateTime.UtcNow.AddDays(7), DateTime.UtcNow, cancellationToken);
            var userPermissions = await _userRepository.GetUserPermissions(userId, cancellationToken);
            var accessToken = _accessTokenGenerator.Generate(user, userPermissions);

            return new RefreshTokenResponse(accessToken, newRefreshToken);
                
        }
    }
}
