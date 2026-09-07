namespace HRMS.Api.Contracts.Positions;

public record CreatePositionRequest(
    string Title,
    string? Description
);
