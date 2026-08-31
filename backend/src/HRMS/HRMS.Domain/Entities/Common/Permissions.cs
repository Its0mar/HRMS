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
            public const string ViewSensitiveData = "employees.sensitive-data.view";
        }

        public static class Attendance
        {
            public const string ClockIn = "attendance.clockIn";
            public const string ClockOut = "attendance.clockOut";
            public const string View = "attendance.view";
        }

        public static class AttendanceCorrections
        {
            public const string View = "attendance_corrections.view";
            public const string ApproveAndReject = "attendance_corrections.aprroveAndReject";
            public const string Update = "attendance_corrections.update";
            public const string Delete = "attendance_corrections.delete";
            public const string Submit = "attendance_corrections.submit";
        }

        public static class LeaveRequests
        {
            public const string ApproveAndReject = "leaves_leaveRequest.ApproveAndReject";
            public const string View = "leaves_leaveRequest.View";
            public const string Submit = "leaves_leaveRequest.Submit";
        }

        public static class LeaveTypes
        {
            public const string Create = "leaves_leaveTypes.Create";
            public const string View = "leaves_leaveTypes.View";
            public const string Update = "leaves_leaveTypes.Update";
        }

        public static class Roles
        {
            public const string Create = "roles.create";
            public const string Update = "roles.update";
            public const string View = "roles.view";
        }

        public static class SystemPermissions
        {
            public const string View = "permissions.view";
        }

        public static class WorkSchedules
        {
            public const string Manage = "workSchedules.manage";
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
            Employees.ViewSensitiveData,

            Attendance.ClockIn,
            Attendance.ClockOut,
            Attendance.View,

            AttendanceCorrections.View,
            AttendanceCorrections.ApproveAndReject,
            AttendanceCorrections.Update,
            AttendanceCorrections.Delete,
            AttendanceCorrections.Submit,

            LeaveRequests.ApproveAndReject,
            LeaveRequests.View,
            LeaveRequests.Submit,

            LeaveTypes.Create,
            LeaveTypes.View,
            LeaveTypes.Update,

            Roles.Create,
            Roles.Update,
            Roles.View,

            SystemPermissions.View,

            WorkSchedules.Manage
        ];
    }
}
