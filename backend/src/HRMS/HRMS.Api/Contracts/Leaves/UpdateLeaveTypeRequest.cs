namespace HRMS.Api.Contracts.Leaves;

public record UpdateLeaveTypeRequest(
    int Id,
    string Name,
    string Code,
    int DefaultDaysPerYear,
    bool IsPaid,
    bool RequiresApproval
);
