using HRMS.Application.Abstractions.Messaging;


namespace HRMS.Application.Features.Positions.CreatePosition
{
    public sealed record CreatePositionCommand(
        string Title,
        string? Description
        ) : ICommand<int>;
}