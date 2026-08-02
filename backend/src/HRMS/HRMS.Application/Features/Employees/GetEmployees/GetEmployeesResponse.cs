using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Employees.GetEmployees
{
    public record GetEmployeesResponse(
        int Id,
        string EmployeeNumber,
        string FullName,
        string WorkEmail,
        string DepartmentName,
        string PositionName,
        string EmploymentType,
        string EmploymentStatus);
}