using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.Payroll.Enums;
using System.Globalization;

namespace HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun
{
    public sealed class CalculatePayrollRunCommandHandler(
        IPayrollRepository payrollRepository,
        ICurrentUser currentUser) : ICommandHandler<CalculatePayrollRunCommand, int>
    {
        public async Task<ErrorOr<int>> HandleAsync(
            CalculatePayrollRunCommand command,
            CancellationToken cancellationToken)
        {
            var organizationId = currentUser.OrganizationId;
            // 1. Resolve Period Start and End Dates automatically
            var (startDate, endDate) = ResolvePeriodDates(command);
            // 2. Prevent Duplicate Payroll Runs
            var exists = await payrollRepository.PayrollRunExistsAsync(
                organizationId,
                command.Year,
                command.Month,
                command.Week,
                command.PayFrequency,
                cancellationToken);
            if (exists)
            {
                return Error.Conflict(
                    "Payroll.AlreadyCalculated",
                    "Payroll for this period has already been processed.");
            }
            // 3. Fetch calculation data for all active employees in 1 DB call
            var employeesData = await payrollRepository.GetEmployeesForCalculationAsync(
                organizationId,
                command.PayFrequency,
                startDate,
                endDate,
                cancellationToken);
            if (employeesData.Count == 0)
            {
                return Error.NotFound(
                    "Payroll.NoEmployeesFound",
                    "No active employees found for the selected pay frequency.");
            }
            // 4. Instantiate Aggregate Root
            var payrollRun = new PayrollRun(
                organizationId,
                command.PayFrequency,
                command.Year,
                command.Month,
                command.Week,
                startDate,
                endDate);
            // 5. Build Employee Payslip Line Items
            foreach (var emp in employeesData)
            {
                var item = new PayrollRunItem(
                    0,
                    emp.EmployeeId,
                    emp.BasicSalary,
                    emp.TotalWorkingDays,
                    emp.UnpaidDays,
                    emp.TotalAllowances,
                    emp.TotalFixedDeductions);
                var addResult = payrollRun.AddItem(item);
                if (addResult.IsError)
                    return addResult.Errors;
            }
            // 6. Persist to Database atomically (TVP)
            var payrollRunId = await payrollRepository.CreatePayrollRunAsync(payrollRun, cancellationToken);
            return payrollRunId;
        }
        private static (DateTime StartDate, DateTime EndDate) ResolvePeriodDates(CalculatePayrollRunCommand command)
        {
            if (command.PayFrequency == PayFrequency.Monthly)
            {
                var start = new DateTime(command.Year, command.Month!.Value, 1);
                var daysInMonth = DateTime.DaysInMonth(command.Year, command.Month.Value);
                var end = new DateTime(command.Year, command.Month.Value, daysInMonth);
                return (start, end);
            }
            // Weekly: Monday to Sunday
            var monday = ISOWeek.ToDateTime(command.Year, command.Week!.Value, DayOfWeek.Monday);
            var sunday = monday.AddDays(6);
            return (monday, sunday);
        }
    }
}
