namespace HRMS.Domain.Entities.Roles
{
    public class Permission
    {
        public int Id { get; private set; }
        public string Code { get; private set; }

        public Permission(int id, string code)
        {
            Id = id;
            Code = code;
        }
    }
}
