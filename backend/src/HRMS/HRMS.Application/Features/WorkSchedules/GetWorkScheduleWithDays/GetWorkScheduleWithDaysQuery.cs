using HRMS.Application.Abstractions.Messaging;


namespace HRMS.Application.Features.WorkSchedules.GetWorkScheduleWithDays
{
    public sealed record GetWorkScheduleWithDaysQuery(int Id) : IQuery<WorkScheduleWithDaysResponse>;
}
