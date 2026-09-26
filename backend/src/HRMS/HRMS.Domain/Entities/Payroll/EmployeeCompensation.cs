using HRMS.Domain.Entities.Payroll.Enums;

namespace HRMS.Domain.Entities.Payroll
{
    public class EmployeeCompensation
    {
        public int? Id { get; private set; }
        public int OrganizationId { get; private set; }
        public int EmployeeId { get; private set; }
        public decimal BasicSalary { get; private set; }
        public string Currency { get; private set; } = "JOD";
        public PayFrequency PayFrequency { get; private set; } = PayFrequency.Monthly;
        public DateTime EffectiveFrom { get; private set; } = DateTime.UtcNow;
        public DateTime? EffectiveTo { get; private set; }
        public bool IsCurrent { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        private readonly List<CompensationAllowance> _allowances = new();

        public IReadOnlyCollection<CompensationAllowance> Allowances => _allowances.AsReadOnly();

        public EmployeeCompensation(int organizationId, int employeeId, decimal basicSalary, string currency, PayFrequency payFrequency)
        {
            OrganizationId = organizationId;
            EmployeeId = employeeId;
            BasicSalary = basicSalary;
            Currency = currency;
            PayFrequency = payFrequency;
        }

        public void AddAllowance(string title, decimal amount)
        {
            _allowances.Add(new CompensationAllowance(Id ?? 0, title, amount));
        }

        public void AddAllowances(IEnumerable<CompensationAllowance> allowances)
        {
            _allowances.AddRange(allowances);
        }

        public void Deactivate(DateTime effectiveToDate)
        {
            IsCurrent = false;
            EffectiveTo = effectiveToDate;
        }

        public static EmployeeCompensation Restore(
            int? id,
            int organizationId,
            int employeeId,
            decimal basicSalary,
            string currency,
            PayFrequency payFrequency,
            DateTime effectiveFrom,
            DateTime? effectiveTo,
            bool isCurrent,
            DateTime createdAt,
            IEnumerable<CompensationAllowance>? allowances = null)
        {
            var compensation = new EmployeeCompensation(organizationId, employeeId, basicSalary, currency, payFrequency)
            {
                Id = id,
                EffectiveFrom = effectiveFrom,
                EffectiveTo = effectiveTo,
                IsCurrent = isCurrent,
                CreatedAt = createdAt
            };

            if (allowances is not null)
            {
                compensation.AddAllowances(allowances);
            }

            return compensation;
        }
    }
}