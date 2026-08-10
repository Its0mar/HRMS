using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Employees.GetEmployeeAccess
{
    public record GetEmployeeAccessQuery(int EmployeeId, int OrganizationId)
        : IQuery<GetEmployeeAccessResponse>;
}
