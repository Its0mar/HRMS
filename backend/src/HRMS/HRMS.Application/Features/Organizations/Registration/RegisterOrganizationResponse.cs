namespace HRMS.Application.Features.Organizations.Registration
{
    public sealed record RegisterOrganizationResponse(
        int OrganizationId,
        int OwnerUserId);
}
