using Asp.Versioning;
using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Attendance.ClockIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class AttendanceController(
        ICommandDispatcher commandDispatcher,
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
    }
}
