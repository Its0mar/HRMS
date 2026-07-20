using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Positions.CreatePosition;
using HRMS.Application.Features.Positions.GetPositions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ApiController
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(
            CreatePositionCommand command,
            [FromServices] ICommandHandler<CreatePositionCommand, int> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.Match(
                ok => StatusCode(StatusCodes.Status201Created, new { id = result.Value }),
                Problem);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            GetPositionsQuery query,
            [FromServices] IQueryHandler<GetPositionsQuery, List<GetPositionResponse>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.Match(
                ok => Ok(result.Value),
                Problem);
        }
    }
}
