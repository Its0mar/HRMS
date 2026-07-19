using ErrorOr;

namespace HRMS.Application.Features.Authentication
{
    public static class AuthenticationErrors
    {
        public static Error InvalidCredentials =>
            Error.Unauthorized(
                code: "Authentication.InvalidCredentials",
                description: "Invalid username/email or password.");

        public static Error OrganizationCodeExists =>
            Error.Conflict(
                code: "Organization.CodeExists",
                description: "An organization with this code already exists.");

        public static Error OrganizationEmailExists =>
            Error.Conflict(
                code: "Organization.EmailExists",
                description: "An organization with this email already exists.");

        public static Error UserEmailExists =>
            Error.Conflict(
                code: "User.EmailExists",
                description: "A user with this email already exists.");

        public static Error UsernameExists =>
            Error.Conflict(
                code: "User.UsernameExists",
                description: "This username is already in use.");
    }
}
