using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.WorkSchedules.AssignEmployee
{
    public record AssignEmployeeCommand(
        int EmployeeId,
        int WorkScheduleId,
        int OrganizationId) : ICommand<bool>;
}
