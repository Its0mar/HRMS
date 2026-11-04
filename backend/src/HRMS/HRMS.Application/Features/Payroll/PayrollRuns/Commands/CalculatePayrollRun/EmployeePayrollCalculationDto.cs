namespace HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun
{
    public class EmployeePayrollCalculationDto
    {
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalFixedDeductions { get; set; }
        public int UnpaidDays { get; set; }
        public int TotalWorkingDays { get; set; }
    }
}