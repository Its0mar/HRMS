using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Authentication.RefreshToken
{
    public record RefreshTokenCommand(
        string refreshToken)
        : ICommand<RefreshTokenResponse>;
}
