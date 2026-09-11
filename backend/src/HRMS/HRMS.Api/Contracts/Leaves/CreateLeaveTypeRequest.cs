namespace HRMS.Api.Contracts.Leaves;

public record CreateLeaveTypeRequest(
    string Name,
    string Code,
    int DefaultDaysPerYear,
    bool IsPaid,
    bool RequiresApproval
);
