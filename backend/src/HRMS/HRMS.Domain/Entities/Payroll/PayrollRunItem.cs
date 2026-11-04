
namespace HRMS.Domain.Entities.Payroll
{
    public class PayrollRunItem
    {
        public int? Id { get; private set; }
        public int PayrollRunId { get; private set; }
        public int EmployeeId { get; private set; }
        public decimal BasicSalary { get; private set; }
        public int TotalWorkingDays { get; private set; }
        public int UnpaidDays { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal AttendanceDeductionAmount { get; private set; }
        public decimal TotalAllowances { get; private set; }
        public decimal TotalFixedDeductions { get; private set; }
        public decimal GrossSalary { get; private set; }
        public decimal NetSalary { get; private set; }

        private PayrollRunItem() { }

        public PayrollRunItem(
            int payrollRunId,
            int employeeId,
            decimal basicSalary,
            int totalWorkingDays,
            int unpaidDays,
            decimal totalAllowances,
            decimal totalFixedDeductions)
        {
            PayrollRunId = payrollRunId;
            EmployeeId = employeeId;
            BasicSalary = basicSalary;
            TotalWorkingDays = totalWorkingDays;
            UnpaidDays = unpaidDays;
            TotalAllowances = totalAllowances;
            TotalFixedDeductions = totalFixedDeductions;


            DailyRate  = TotalWorkingDays > 0 ? Math.Round(BasicSalary / TotalWorkingDays, 2, MidpointRounding.AwayFromZero) : 0;
            AttendanceDeductionAmount = Math.Round(DailyRate * UnpaidDays, 2, MidpointRounding.AwayFromZero);
            
            GrossSalary = BasicSalary + TotalAllowances;

            var calculatedNet = GrossSalary - TotalFixedDeductions - AttendanceDeductionAmount;
            NetSalary = Math.Max(0, calculatedNet);

        }

        public static PayrollRunItem Restore(
            int id,
            int payrollRunId,
            int employeeId,
            decimal basicSalary,
            int totalWorkingDays,
            int unpaidDays,
            decimal dailyRate,
            decimal attendanceDeductionAmount,
            decimal totalAllowances,
            decimal totalFixedDeductions,
            decimal grossSalary,
            decimal netSalary)
        {
            return new PayrollRunItem
            {
                Id = id,
                PayrollRunId = payrollRunId,
                EmployeeId = employeeId,
                BasicSalary = basicSalary,
                TotalWorkingDays = totalWorkingDays,
                UnpaidDays = unpaidDays,
                DailyRate = dailyRate,
                AttendanceDeductionAmount = attendanceDeductionAmount,
                TotalAllowances = totalAllowances,
                TotalFixedDeductions = totalFixedDeductions,
                GrossSalary = grossSalary,
                NetSalary = netSalary
            };
        }


    }
}

