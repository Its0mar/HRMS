
using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.WorkSchedules.GetWorkScheduleOptions
{
    public record GetWorkScheduleOptionsQuery(
        ) : IQuery<IReadOnlyList<GetWorkScheduleOptionsResponse>>;
}
