namespace HRMS.Application.Features.Authentication.RefreshToken
{
    public record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken,
        DateTime RefreshTokenExpiresAt);
}
