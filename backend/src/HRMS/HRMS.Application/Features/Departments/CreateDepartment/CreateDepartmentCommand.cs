using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Departments.CreateDepartment
{
    public sealed record CreateDepartmentCommand(
        string Name,
        string Code,
        string? Description,
        int? ManagerId,
        int OrganizationId)
    : ICommand<CreateDepartmentResponse>, ICacheEvictingCommand
    {
        public string CacheKeyToEvict => $"depts:org:{OrganizationId}";
    }
}
