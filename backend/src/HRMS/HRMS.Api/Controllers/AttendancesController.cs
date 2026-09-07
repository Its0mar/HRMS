using Asp.Versioning;
using HRMS.Api.Contracts.Attendance;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Attendance.AttendanceCorrections.AttendanceCorrectionApprove;
using HRMS.Application.Features.Attendance.AttendanceCorrections.GetOrganizationAttendanceCorrection;
using HRMS.Application.Features.Attendance.AttendanceCorrections.SubmitCorrection;
using HRMS.Application.Features.Attendance.ClockIn;
using HRMS.Application.Features.Attendance.ClockOut;
using HRMS.Application.Features.Attendance.GetEmployeeAttendance;
using HRMS.Application.Features.Attendance.GetOrganizationAttendance;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class AttendancesController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ICurrentUser currentUser) : ApiController
    {
        [Authorize(Policy = Permissions.Attendance.ClockIn)]
        [HttpPost("clock-in")]
        public async Task<IActionResult> ClockIn(CancellationToken cancellationToken)
        {
            var command = new ClockInCommand(currentUser.EmployeeId);
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            
            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize(Policy = Permissions.Attendance.ClockOut)]
        [HttpPost("clock-out")]
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

        [Authorize(Policy = Permissions.AttendanceCorrections.Submit)]
        [HttpPost("corrections")]
        public async Task<IActionResult> Correct([FromBody] SubmitCorrectionRequest request, CancellationToken cancellationToken)
        {
            var command = new SubmitCorrectionCommand(
                request.AttendanceLogId,
                request.RequestedClockIn,
                request.RequestedClockOut,
                request.Reason);

            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize(Policy = Permissions.Attendance.View)]
        [HttpGet("organization")]
        public async Task<IActionResult> GetOrganizationAttendance(
            [FromQuery] DateOnly? date,
            [FromQuery] string? searchTerm,
            CancellationToken cancellationToken)
        {
            var query = new GetOrganizationAttendanceQuery(date, searchTerm);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.AttendanceCorrections.View)]
        [HttpGet("corrections/organization")]
        public async Task<IActionResult> GetOrganizationAttendanceCorrection(
            [FromQuery] GetOrganizationAttendanceCorrectionQuery query,
            CancellationToken cancellationToken)
        {
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.AttendanceCorrections.ApproveAndReject)]
        [HttpPost("corrections/approve")]
        public async Task<IActionResult> Approve(
            AttendanceCorrectionApproveCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                ok => Ok(),
                Problem);
        }
    }
}
