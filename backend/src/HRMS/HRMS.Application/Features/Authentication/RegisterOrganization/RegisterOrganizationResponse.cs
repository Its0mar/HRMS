namespace HRMS.Application.Features.Authentication.RegisterOrganization
{
    public sealed record RegisterOrganizationResponse(
        int OrganizationId,
        int OwnerUserId);
}
