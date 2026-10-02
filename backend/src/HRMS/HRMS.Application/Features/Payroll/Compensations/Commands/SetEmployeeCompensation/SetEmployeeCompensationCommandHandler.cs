using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Payroll;

namespace HRMS.Application.Features.Payroll.Compensations.Commands.SetEmployeeCompensation
{
    public sealed class SetEmployeeCompensationCommandHandler(IPayrollRepository repository, ICurrentUser user) : ICommandHandler<SetEmployeeCompensationCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(SetEmployeeCompensationCommand command, CancellationToken cancellationToken)
        {
            var employeeCompensation = ToEmployeeCompensation(command);

            return await repository.SetCompensationAsync(employeeCompensation, cancellationToken);
        }

        private EmployeeCompensation ToEmployeeCompensation(SetEmployeeCompensationCommand command)
        {
            var employeeCompensation = new EmployeeCompensation(
                user.OrganizationId,
                command.EmployeeId,
                command.BasicSalary,
                command.Currency,
                command.PayFrequency,
                command.EffectiveFrom);

            foreach (var allowance in command.Allowances)
            {
                employeeCompensation.AddAllowance(allowance.Title, allowance.Amount);
            }

            return employeeCompensation;
        }
    }
}
