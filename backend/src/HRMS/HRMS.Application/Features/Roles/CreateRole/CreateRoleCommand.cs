using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.CreateRole
{
    public record CreateRoleCommand(
        string Name,
        List<int> PermissionIds,
        int OrganizationId) : ICommand<bool>, ICacheEvictingCommand
    {
        public string CacheKeyToEvict => $"roles:org:{OrganizationId}";
    }
}
