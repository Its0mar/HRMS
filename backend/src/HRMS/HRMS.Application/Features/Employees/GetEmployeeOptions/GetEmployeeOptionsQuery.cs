using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Employees.GetEmployeeOptions
{
    public sealed record GetEmployeeOptionsQuery
    : IQuery<IReadOnlyList<EmployeeOptionResponse>>;
}
