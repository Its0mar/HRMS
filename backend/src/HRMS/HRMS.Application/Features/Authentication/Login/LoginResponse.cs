namespace HRMS.Application.Features.Authentication.Login
{
    public record LoginResponse(
        AuthenticatedUserResponse User,
        string AccessToken,
        string RefreshToken,
        DateTime RefreshTokenExpiresAt);
}
