using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.GetRolesOptions
{
    public record GetRolesOptionsQuery(int OrganizationId) : IQuery<IReadOnlyList<GetRolesOptionsResponse>>, ICachedQuery
    {
        public string CacheKey => $"rolesoptions:org:{OrganizationId}";
        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }
}
