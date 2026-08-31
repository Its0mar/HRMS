export interface CompanyLeaveRequestItem {
    id: number;
    employeeId: number;
    employeeName: string;
    employeeCode: string;
    departmentName: string | null;
    leaveTypeId: number;
    leaveTypeName: string;
    startDate: string;
    endDate: string;
    totalDays: number;
    reason: string;
    status: number;
    reviewedByName: string | null;
    reviewedAt: string | null;
    rejectionReason: string | null;
}