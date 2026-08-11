export interface EmployeeListItem {
    id: number;
    employeeNumber: string;
    fullName: string;
    workEmail: string;
    departmentName: string | null;
    positionName: string | null;
    employmentType: string;
    employmentStatus: string;
    hasUserAccess: boolean;
}