
namespace HRMS.Domain.Entities.Leaves
{
    public class EmployeeLeaveBalances
    {
        public int? Id { get; private set; }
        public int EmployeeId { get; private set; }
        public int LeaveTypeId { get; private set; }
        public int Year { get; private set; }
        public int TotalEntitledDays { get; private set; }
        public int UsedDays { get; private set; }
        public int PendingDays { get; private set; }


        public EmployeeLeaveBalances(int employeeId, int leaveTypeId, int year, int totalEntitledDays, int usedDays, int pendingDays)
        {
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;
            Year = year;
            TotalEntitledDays = totalEntitledDays;
            UsedDays = usedDays;
            PendingDays = pendingDays;
        }

        public static EmployeeLeaveBalances Restore(int? id, int employeeId, int leaveTypeId, int year, int totalEntitledDays, int usedDays, int pendingDays)
        {
            return new EmployeeLeaveBalances(employeeId, leaveTypeId, year, totalEntitledDays, usedDays, pendingDays)
            {
                Id = id
            };
        }
    }
}