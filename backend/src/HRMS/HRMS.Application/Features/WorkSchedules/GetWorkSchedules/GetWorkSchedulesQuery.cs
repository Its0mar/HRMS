using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.WorkSchedules.GetWorkSchedules
{
    public record  GetWorkSchedulesQuery() : IQuery<List<WorkScheduleResponse>>;
}
