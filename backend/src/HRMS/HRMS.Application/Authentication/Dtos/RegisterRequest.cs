namespace HRMS.Application.Authentication.Dtos
{
    public sealed record RegisterRequest(
    string OrganizationName,
    string OrganizationCode,
    string OrganizationEmail,
    string? Address,
    string? Website,
    string? LogoUrl,
    string OwnerUsername,
    string OwnerEmail,
    string Password,
    string FirstName,
    string LastName);
}
