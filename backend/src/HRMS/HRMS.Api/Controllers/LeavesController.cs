using Asp.Versioning;
using HRMS.Api.Contracts.Leaves;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Leaves.LeaveBalances.GetLeaveBalances;
using HRMS.Application.Features.Leaves.LeaveRequests.ApproveLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveRequests.CancelPending;
using HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests;
using HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests;
using HRMS.Application.Features.Leaves.LeaveRequests.RejectLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveRequests.SubmitLeaveRequest;
using HRMS.Application.Features.Leaves.LeaveTypes.CreateLeaveType;
using HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes;
using HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class LeavesController(
         ICommandDispatcher commandDispatcher,
         IQueryDispatcher queryDispatcher,
         ICurrentUser currentUser) : ApiController
    {
        [Authorize(Policy = Permissions.LeaveTypes.Create)]
        [HttpPost("types")]
        public async Task<IActionResult> Create(
            [FromBody] CreateLeaveTypeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateLeaveTypeCommand(
                request.Name,
                request.Code,
                request.DefaultDaysPerYear,
                request.IsPaid,
                request.RequiresApproval,
                currentUser.OrganizationId);

            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveTypes.View)]
        [HttpGet("types")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var query = new GetLeaveTypesQuery(currentUser.OrganizationId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveTypes.Update)]
        [HttpPut("types")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateLeaveTypeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLeaveTypeCommand(
                request.Id,
                request.Name,
                request.Code,
                request.DefaultDaysPerYear,
                request.IsPaid,
                request.RequiresApproval,
                currentUser.OrganizationId);

            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet("balances")]
        public async Task<IActionResult> GetMyLeaveBalances([FromQuery] int year, CancellationToken cancellationToken)
        {
            var query = new GetLeaveBalancesQuery(year, currentUser.EmployeeId, currentUser.OrganizationId);

            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.Submit)]
        [HttpPost("requests")]
        public async Task<IActionResult> Apply([FromBody] ApplyLeaveRequest request, CancellationToken cancellationToken)
        {
            var command = new SubmitLeaveRequestCommand(
                request.LeaveTypeId,
                request.StartDate,
                request.EndDate,
                request.Reason);

            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [Authorize]
        [HttpGet("requests/my")]
        public async Task<IActionResult> MyRequests(CancellationToken cancellationToken)
        {
            var query = new GetMyLeaveRequestsQuery(currentUser.EmployeeId, currentUser.OrganizationId);
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.View)]
        [HttpGet("requests/organization")]
        public async Task<IActionResult> OrganizationRequests([FromQuery] GetOrganizationLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            var result = await queryDispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                Ok,
                Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.ApproveAndReject)]
        [HttpPost("requests/approve")]
        public async Task<IActionResult> Approve(
            ApproveLeaveRequestCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            return result.Match(_ => Ok(), Problem);
        }

        [Authorize(Policy = Permissions.LeaveRequests.ApproveAndReject)]
        [HttpPost("requests/reject")]
        public async Task<IActionResult> Reject(
            RejectLeaveRequestCommand command,
            CancellationToken cancellationToken)
        {
            var result = await commandDispatcher.SendAsync(command, cancellationToken);
            return result.Match(_ => Ok(), Problem);
        }

        [Authorize]
        [HttpPut("requests/{id:int}/cancel")]
        public async Task<IActionResult> Cancel(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new CancelPendingCommand(id, currentUser.EmployeeId, currentUser.OrganizationId);
            var result = await commandDispatcher.SendAsync(command, cancellationToken);

            return result.Match(_ => Ok(), Problem);
        }
    }
}
