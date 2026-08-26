
using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Leaves;
using HRMS.Domain.Entities.WorkSchedules.Enums;

namespace HRMS.Application.Features.Leaves.LeaveRequests.SubmitLeaveRequest
{
    public sealed class SubmitLeaveRequestHandler(
        ILeaveRepository leaveRepository,
        IWorkScheduleRepository workScheduleRepository,
        ICurrentUser currentUser)
        : ICommandHandler<SubmitLeaveRequestCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(SubmitLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            //todo: check for overlap
            //todo : check for holidays (if specified in future)

            //get balance
            var currentYear = DateTime.Now.Year;
            var balance = await leaveRepository.GeyMyBalanceAsync(command.LeaveTypeId, currentYear, currentUser.EmployeeId, cancellationToken);
            var workSchedule = await workScheduleRepository.GetEmployeeWorkScheduleByEmployeeId(currentUser.EmployeeId, currentUser.OrganizationId, cancellationToken);

            int totalEntitledDays = 0;

            if (workSchedule is null) return Error.Failure();

            if (balance is null)
            {
                var leaveType = await leaveRepository.GetTypeByIdAsync(command.LeaveTypeId, currentUser.OrganizationId, cancellationToken);

                if (leaveType is null) return Error.Failure();

                totalEntitledDays = leaveType.DefaultDaysPerYear;
            }

            else totalEntitledDays = balance.TotalEntitledDays;

            var remainingDays = balance is null
                ? totalEntitledDays
                : (balance.TotalEntitledDays - balance.UsedDays - balance.PendingDays);

            //validate the requested with balance
            int requestedDays = 0;

            for (var start = command.StartDate; start <= command.EndDate; start = start.AddDays(1))
            {
                var day = workSchedule.Days.FirstOrDefault(ws => ws.WorkDay == ToWorkDay(start.DayOfWeek));
                if (day is not null && day.IsWorkingDay) requestedDays++;
            }

            if (requestedDays <= 0)
            {
                return Error.Failure(description: "you must choose atleast one working day");
            }

            if (requestedDays > remainingDays)
            {
                return Error.Conflict(description: "you requested more days than available");
            }


            //update balanace or create it

            if (balance is null)
            {
                var empBalance = new EmployeeLeaveBalance(currentUser.EmployeeId, command.LeaveTypeId, currentYear, totalEntitledDays, 0, requestedDays);
                var result = await leaveRepository.CreateEmployeeBalance(empBalance, cancellationToken);

                if (!result) return Error.Failure(description: "could not create the balance");
            }
            else
            {
                balance.UpdatePendingDays(requestedDays);
                var result = await leaveRepository.UpdatePendingDaysAsync(balance, cancellationToken);
                if (!result) return Error.Failure(description: "could not create the balance");

            }

            //create leave request
            var leaveRequest = new LeaveRequest(currentUser.OrganizationId, currentUser.EmployeeId, command.LeaveTypeId, command.StartDate, command.EndDate, requestedDays, command.Reason);
            await leaveRepository.CreateLeaveRequestAsync(leaveRequest, cancellationToken);

            return true;

        }

        private WorkDay ToWorkDay(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday) return WorkDay.Sunday;
            if (dayOfWeek == DayOfWeek.Monday) return WorkDay.Monday;
            if (dayOfWeek == DayOfWeek.Tuesday) return WorkDay.Tuesday;
            if (dayOfWeek == DayOfWeek.Wednesday) return WorkDay.Wednesday;
            if (dayOfWeek == DayOfWeek.Thursday) return WorkDay.Thursday;
            if (dayOfWeek == DayOfWeek.Friday) return WorkDay.Friday;
            return WorkDay.Saturday;
        } 
    }
}
