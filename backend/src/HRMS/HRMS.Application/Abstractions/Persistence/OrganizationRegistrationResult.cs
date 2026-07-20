namespace HRMS.Application.Abstractions.Persistence
{
    public sealed record OrganizationRegistrationResult(
        int OrganizationId,
        int OwnerUserId);
}
