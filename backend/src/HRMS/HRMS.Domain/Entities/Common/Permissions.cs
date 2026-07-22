namespace HRMS.Domain.Entities.Common
{
    public static class Permissions
    {
        public static class Departments
        {
            public const string View = "departments.view";
            public const string Create = "departments.create";
            public const string Update = "departments.update";
            public const string Delete = "departments.delete";
        }

        public static class Positions
        {
            public const string View = "positions.view";
            public const string Create = "positions.create";
            public const string Update = "positions.update";
            public const string Delete = "positions.delete";
        }

        public static class Employees
        {
            public const string View = "employees.view";
            public const string Create = "employees.create";
            public const string Update = "employees.update";
            public const string Delete = "employees.delete";
            public const string ViewSensitiveData =
                "employees.sensitive-data.view";
        }

        public static readonly string[] All =
        [
            Departments.View,
            Departments.Create,
            Departments.Update,
            Departments.Delete,

            Positions.View,
            Positions.Create,
            Positions.Update,
            Positions.Delete,

            Employees.View,
            Employees.Create,
            Employees.Update,
            Employees.Delete,
            Employees.ViewSensitiveData
        ];
    }

}
