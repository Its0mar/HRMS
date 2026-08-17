using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Authentication.Logout
{
    public sealed class LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator) : ICommandHandler<LogoutCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(
            LogoutCommand command,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.RefreshToken)) return true;

            var tokenHash = refreshTokenGenerator.Hash(command.RefreshToken);
            await refreshTokenRepository.RevokeAsync(tokenHash, DateTime.UtcNow, cancellationToken);

            return true;
        }
    }
}
