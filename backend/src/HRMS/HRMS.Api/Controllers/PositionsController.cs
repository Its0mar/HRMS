using Asp.Versioning;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Positions.CreatePosition;
using HRMS.Application.Features.Positions.GetPositions;
using HRMS.Domain.Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    public class PositionsController : ApiController
    {
        [HttpPost]
        [Authorize(Policy = Permissions.Positions.Create)]
        public async Task<IActionResult> CreateAsync(
            CreatePositionCommand command,
            [FromServices] ICommandDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);

            return result.Match(
                ok => StatusCode(StatusCodes.Status201Created, new { id = result.Value }),
                Problem);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Positions.View)]
        public async Task<IActionResult> GetAsync(
            [FromServices] IQueryDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            var query = new GetPositionsQuery();
            var result = await dispatcher.SendAsync(query, cancellationToken);

            return result.Match(
                ok => Ok(result.Value),
                Problem);
        }
    }
}
