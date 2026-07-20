using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;


namespace HRMS.Application.Features.Departments.GetDepartments
{
    public sealed record GetDepartmentsQuery() : IQuery<List<GetDepartmentResponse>>;

    public class GetDepartmentsQueryHandler
        : IQueryHandler<GetDepartmentsQuery, List<GetDepartmentResponse>>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICurrentUser _currentUser;

        public GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository, ICurrentUser currentUser)
        {
            _departmentRepository = departmentRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<List<GetDepartmentResponse>>> HandleAsync(GetDepartmentsQuery query, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetDepartmentsAsync(_currentUser.OrganizationId, cancellationToken);

            return departments.Select(d => new GetDepartmentResponse(d.Id ?? -1, d.Name)).ToList();
        }
    }
}
