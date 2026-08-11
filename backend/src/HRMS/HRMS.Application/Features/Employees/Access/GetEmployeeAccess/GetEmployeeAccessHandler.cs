using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Employees.Access.GetEmployeeAccess
{
    public sealed class GetEmployeeAccessHandler : IQueryHandler<GetEmployeeAccessQuery, GetEmployeeAccessResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeAccessHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ErrorOr<GetEmployeeAccessResponse>> HandleAsync(GetEmployeeAccessQuery query, CancellationToken cancellationToken)
        {
            var access = await _employeeRepository.GetAccessByEmployeeIdAsync(query.EmployeeId, query.OrganizationId, cancellationToken);
            if (access is null)
            {
                return Error.NotFound(description: "user access is not found");
            }

            return new GetEmployeeAccessResponse(
                access.UserId, access.EmployeeId, access.RoleId, access.UserName);
        }
    }
}
