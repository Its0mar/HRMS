using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Authentication.Logout
{
    public sealed class LogoutCommandHandler
    : ICommandHandler<LogoutCommand, bool>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public LogoutCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenGenerator refreshTokenGenerator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<ErrorOr<bool>> HandleAsync(
            LogoutCommand command,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.RefreshToken))
            {
                return true;
            }

            var tokenHash = _refreshTokenGenerator.Hash(command.RefreshToken);

            await _refreshTokenRepository.RevokeAsync(
                tokenHash,
                DateTime.UtcNow,
                cancellationToken);

            return true;
        }
    }
}
