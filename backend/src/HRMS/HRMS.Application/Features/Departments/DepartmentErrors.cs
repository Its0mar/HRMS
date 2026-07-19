using ErrorOr;

namespace HRMS.Application.Features.Departments
{
    public static class DepartmentErrors
    {
        public static Error NameExists =>
            Error.Conflict(
                code: "Department.NameExists",
                description: "A department with this name already exists.");

        public static Error CodeExists =>
            Error.Conflict(
                code: "Department.CodeExists",
                description: "A department with this code already exists.");

        public static Error CreationFailed =>
            Error.Failure(
                code: "Department.CreationFailed",
                description: "The department could not be created.");
    }
}
