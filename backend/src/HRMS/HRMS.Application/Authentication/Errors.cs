using ErrorOr;

namespace HRMS.Application.Authentication
{
    public static class AuthErrors
    {
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
