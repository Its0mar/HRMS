namespace HRMS.Domain.Entities.Leaves
{
    public class LeaveType
    {
        public int? Id { get; private set; }
        public int OrganizationId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int DefaultDaysPerYear { get; private set; }
        public bool IsPaid { get; private set; }
        public bool RequiresApproval { get; private set; }

        public LeaveType(int organizationId, string name, string code, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
        {
            OrganizationId = organizationId;
            Name = name;
            Code = code;
            DefaultDaysPerYear = defaultDaysPerYear;
            IsPaid = isPaid;
            RequiresApproval = requiresApproval;
        }

        public static LeaveType Restore(int id, int organizationId, string name, string code, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
        {
            return new LeaveType(organizationId, name, code, defaultDaysPerYear, isPaid, requiresApproval)
            {
                Id = id
            };
        }

        public void Update(string name, string code, int defaultDaysPerYear, bool isPaid, bool requiresApproval)
        {
            Name = name;
            Code = code;
            DefaultDaysPerYear = defaultDaysPerYear;
            IsPaid = isPaid;
            RequiresApproval = requiresApproval;
        }
    }
}
