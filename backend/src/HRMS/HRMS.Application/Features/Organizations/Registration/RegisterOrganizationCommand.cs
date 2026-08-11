using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Organizations.Registration;

public sealed record RegisterOrganizationCommand(
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
    string LastName)
    : ICommand<RegisterOrganizationResponse>;