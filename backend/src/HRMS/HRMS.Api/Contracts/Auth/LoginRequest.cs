namespace HRMS.Api.Contracts.Auth;

public record LoginRequest(
    string Identifier,
    string Password
);
