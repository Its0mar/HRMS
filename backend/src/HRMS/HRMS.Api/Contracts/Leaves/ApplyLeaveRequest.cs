namespace HRMS.Api.Contracts.Leaves;

public record ApplyLeaveRequest(
    int LeaveTypeId,
    DateTime StartDate,
    DateTime EndDate,
    string Reason
);
