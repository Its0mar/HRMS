
namespace HRMS.Domain.Entities.Leaves
{
    public class EmployeeLeaveBalance
    {
        public int? Id { get; private set; }
        public int EmployeeId { get; private set; }
        public int LeaveTypeId { get; private set; }
        public int Year { get; private set; }
        public int TotalEntitledDays { get; private set; }
        public decimal UsedDays { get; private set; }
        public decimal PendingDays { get; private set; }


        public EmployeeLeaveBalance(int employeeId, int leaveTypeId, int year, int totalEntitledDays, decimal usedDays, decimal pendingDays)
        {
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;
            Year = year;
            TotalEntitledDays = totalEntitledDays;
            UsedDays = usedDays;
            PendingDays = pendingDays;
        }

        public static EmployeeLeaveBalance Restore(int? id, int employeeId, int leaveTypeId, int year, int totalEntitledDays, decimal usedDays, decimal pendingDays)
        {
            return new EmployeeLeaveBalance(employeeId, leaveTypeId, year, totalEntitledDays, usedDays, pendingDays)
            {
                Id = id
            };
        }

        public void UpdateUsedDays(decimal usedDays) { UsedDays += usedDays; }

        public void UpdatePendingDays(decimal pendingDays) { PendingDays += pendingDays; }
    }
}