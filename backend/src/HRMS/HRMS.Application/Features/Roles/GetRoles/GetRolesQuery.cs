using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.GetRoles
{
    public record GetRolesQuery(int OrganizationId) : IQuery<IReadOnlyList<GetRoleResponse>>, ICachedQuery
    {
        public string CacheKey => $"roles:org:{OrganizationId}";
        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }
}
