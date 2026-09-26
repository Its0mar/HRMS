namespace HRMS.Domain.Entities.Payroll
{
    public class CompensationAllowance
    {
        public int? Id { get; private set; }
        public int CompensationId { get; private set; }
        public string Title { get; private set; }
        public decimal Amount { get; private set; }

        public CompensationAllowance(int compensationId, string title, decimal amount)
        {
            CompensationId = compensationId;
            Title = title;
            Amount = amount;
        }

        public static CompensationAllowance Restore(int id, int compensationId, string title, decimal amount)
        {
            return new CompensationAllowance(compensationId, title, amount)
            {
                Id = id
            };
        }

        public void Update(string title, decimal amount)
        {
            Title = title;
            Amount = amount;
        }
    }
}
