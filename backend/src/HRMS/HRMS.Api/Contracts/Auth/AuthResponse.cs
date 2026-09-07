using HRMS.Application.Features.Authentication.Login;

namespace HRMS.Api.Contracts.Auth;

public record AuthResponse(
    AuthenticatedUserResponse User,
    string AccessToken
);
