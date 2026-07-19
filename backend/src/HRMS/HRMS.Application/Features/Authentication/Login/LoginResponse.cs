namespace HRMS.Application.Features.Authentication.Login
{
    public record LoginResponse(
        AuthenticatedUserResponse User,
        string Token);
}
