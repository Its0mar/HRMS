namespace HRMS.Api.Contracts.Auth;

public record ChangePasswordRequest(
    string OldPassword,
    string NewPassword
);
