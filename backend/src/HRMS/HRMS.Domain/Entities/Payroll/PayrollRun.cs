using ErrorOr;
using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Domain.Entities.Payroll
{
    public class PayrollRun
    {
        public int? Id { get;  private set; }
        public int OrganizationId { get; private set; }
        public PayFrequency PayFrequency { get; private set; }
        public int Year { get; private set; }
        public int? Month { get; private set; }
        public int? Week { get; private set; }
        public DateTime PeriodStartDate { get; private set; }
        public DateTime PeriodEndDate { get; private set; }

        public PayrollRunStatus Status { get; private set; }

        public decimal TotalGrossSalary { get; private set; }
        public decimal TotalAllowances { get; private set; }
        public decimal TotalDeductions { get; private set; }
        public decimal TotalNetSalary { get; private set; }

        private readonly List<PayrollRunItem> _items = new();

        public IReadOnlyCollection<PayrollRunItem> Items => _items.AsReadOnly();

        public DateTime CreatedAt { get; private set; }

        public bool IsApproved => Status == PayrollRunStatus.Approved;
        public DateTime? ApprovedAt { get; private set; }
        public int? ApprovedByEmployeeId { get; private set; }


        public PayrollRun(
            int organizationId,
            PayFrequency payFrequency,
            int year,
            int? month,
            int? week,
            DateTime periodStartDate,
            DateTime periodEndDate)
        {
            OrganizationId = organizationId;
            PayFrequency = payFrequency;
            Year = year;
            Month = month;
            Week = week;
            PeriodStartDate = periodStartDate;
            PeriodEndDate = periodEndDate;
            Status = PayrollRunStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        public static PayrollRun Restore(
            int id,
            int organizationId,
            PayFrequency payFrequency,
            int year,
            int? month,
            int? week,
            DateTime periodStartDate,
            DateTime periodEndDate,
            PayrollRunStatus status,
            decimal totalGrossSalary,
            decimal totalAllowances,
            decimal totalDeductions,
            decimal totalNetSalary,
            DateTime createdAt,
            DateTime? approvedAt,
            int? approvedByEmployeeId,
            IEnumerable<PayrollRunItem>? items = null)
        {
            var payrollRun = new PayrollRun(organizationId, payFrequency, year, month, week, periodStartDate, periodEndDate)
            {
                Id = id,
                Status = status,
                TotalGrossSalary = totalGrossSalary,
                TotalAllowances = totalAllowances,
                TotalDeductions = totalDeductions,
                TotalNetSalary = totalNetSalary,
                CreatedAt = createdAt,
                ApprovedAt = approvedAt,
                ApprovedByEmployeeId = approvedByEmployeeId
            };

            if (items != null)
            {
                payrollRun._items.AddRange(items);
            }

            return payrollRun;
        }

        public ErrorOr<Success> Approve(int approvedByEmployeeId)
        {
            if (Status == PayrollRunStatus.Approved)
                return Error.Conflict("Payroll.AlreadyApproved", "Payroll run is already approved.");

            if (_items.Count == 0)
                return Error.Validation("Payroll.Empty", "Cannot approve a payroll run with no employees.");

            Status = PayrollRunStatus.Approved;
            ApprovedByEmployeeId = approvedByEmployeeId;
            ApprovedAt = DateTime.UtcNow;

            return Result.Success;
        }
        public ErrorOr<Success> AddItem(PayrollRunItem item)
        {
            if (Status != PayrollRunStatus.Draft)
                return Error.Conflict("Payroll.Locked", "Cannot modify items on an approved payroll run.");

            _items.Add(item);
            CalculateTotals();
            return Result.Success;
        }


        private void CalculateTotals()
        {
            TotalGrossSalary = _items.Sum(i => i.GrossSalary);
            TotalAllowances = _items.Sum(i => i.TotalAllowances);
            TotalDeductions = _items.Sum(i => i.TotalFixedDeductions + i.AttendanceDeductionAmount);
            TotalNetSalary = _items.Sum(i => i.NetSalary);
        }


        public ErrorOr<Success> AddItems(IEnumerable<PayrollRunItem> items)
        {
            if (Status != PayrollRunStatus.Draft)
                return Error.Conflict("Payroll.Locked", "Cannot modify items on an approved payroll run.");

            _items.AddRange(items);
            CalculateTotals();
            
            return Result.Success;
        }

        public ErrorOr<Success> RemoveItem(PayrollRunItem item)
        {
            if (Status != PayrollRunStatus.Draft)
                return Error.Conflict("Payroll.Locked", "Cannot modify items on an approved payroll run.");

            _items.Remove(item);
            CalculateTotals();

            return Result.Success;
        }
    }
}