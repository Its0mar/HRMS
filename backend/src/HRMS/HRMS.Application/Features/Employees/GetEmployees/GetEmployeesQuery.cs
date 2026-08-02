using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Employees.GetEmployees
{
    public record GetEmployeesQuery() : IQuery<IReadOnlyList<GetEmployeesResponse>>;
}
