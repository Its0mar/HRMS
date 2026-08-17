using Asp.Versioning;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Attendance.ClockIn;
using HRMS.Application.Features.Attendance.ClockOut;
using HRMS.Application.Features.Attendance.GetUserAttendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class AttendanceController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ICurrentUser currentUser) : ApiController
    {
        [Authorize]
        [HttpPost("ClockIn")]
        public async Task<IActionResult> ClockIn(CancellationToken cancellationToken)
        {
            var command = new ClockInCommand(currentUser.EmployeeId);
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            
            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpPost("ClockOut")]
        public async Task<IActionResult> ClockOut(CancellationToken cancellationToken)
        {
            var command = new ClockOutCommand(currentUser.EmployeeId);
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyAttendance(CancellationToken cancellationToken)
        {
            var query = new GetEmployeeAttendanceQuery(currentUser.EmployeeId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }
    }
}
