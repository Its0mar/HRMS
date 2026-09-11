using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.UpdateRole
{
    public sealed record UpdateRoleCommand(
        int Id,
        string Name,
        List<int> PermissionIds,
        int OrganizationId) : ICommand<bool>, ICacheEvictingCommand
    {
        public string CacheKeyToEvict => $"roles:org:{OrganizationId}";
    }
}
