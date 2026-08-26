
namespace HRMS.Domain.Entities.Leaves
{
    public sealed class LeaveRequest
    {
        public int? Id { get; private set; }
        public int OrganizationId { get; private set; }
        public int EmployeeId { get; private set; }
        public int LeaveTypeId { get; private set; }
        public DateTime StartDate { get; private set;  }
        public DateTime EndDate { get; private set; }
        public decimal TotalDays { get; private set; }
        public string Reason { get; private set; }
        public LeaveRequestStatus Status {  get; private set; }
        public int? ReviewedById { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public string? RejectionReason { get; private set; }

        public LeaveRequest(int organizationId, int employeeId, int leaveTypeId, DateTime startDate, DateTime endDate, decimal totalDays, string reason)
        {
            OrganizationId = organizationId;
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;
            StartDate = startDate;
            EndDate = endDate;
            TotalDays = totalDays;
            Reason = reason;
            Status = LeaveRequestStatus.Pending;
        }

        public LeaveRequest Restore(int? id, int organizationId, int employeeId, int leaveTypeId, DateTime startDate, DateTime endDate, decimal totalDays, 
            string reason, LeaveRequestStatus status, int? reviewedById, DateTime? reviewedAt, string? rejectionReason)
        {
            return new LeaveRequest(organizationId, employeeId, leaveTypeId, startDate, endDate, totalDays, reason)
            {
                Id = id,
                Status = status,
                ReviewedById = reviewedById,
                ReviewedAt = reviewedAt,
                RejectionReason = rejectionReason
            };
        }
    }
}
