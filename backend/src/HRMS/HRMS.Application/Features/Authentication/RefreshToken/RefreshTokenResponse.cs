namespace HRMS.Application.Features.Authentication.RefreshToken
{
    public record RefreshTokenResponse(
        string JwtToken,
        string RefreshToken);
}
