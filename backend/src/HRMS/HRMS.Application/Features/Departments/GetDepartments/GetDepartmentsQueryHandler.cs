using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Departments.GetDepartments
{
    public sealed record GetDepartmentsQuery(int OrganizationId) : IQuery<List<DepartmentListItem>>, ICachedQuery
    {
        public string CacheKey => $"depts:org:{OrganizationId}";
        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }

    public class GetDepartmentsQueryHandler
        : IQueryHandler<GetDepartmentsQuery, List<DepartmentListItem>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<ErrorOr<List<DepartmentListItem>>> HandleAsync(GetDepartmentsQuery query, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetDepartmentsAsync(query.OrganizationId, cancellationToken);
            return departments;
        }
    }
}
