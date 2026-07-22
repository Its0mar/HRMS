using ErrorOr;

namespace HRMS.Application.Features.Positions
{
    public static class PositionsErrors
    {
        public static Error TitleExists =>
            Error.Conflict(
                code: "Positions.TitleExists",
                description: "A position with this title already exists.");
    }
}
