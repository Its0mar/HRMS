
using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Payroll.Compensations.Queries.GetActiveCompensation
{
    public record GetActiveCompensationQuery(int EmployeeId) : IQuery<GetActiveCompensationResponse>;
}
