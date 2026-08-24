export interface CompanyAttendanceItem {
    id: number;
    employeeId: number;
    employeeName: string;
    employeeCode: string;
    departmentName: string | null;
    date: string;
    clockIn: string;
    clockOut: string | null;
    status: string;
    totalMinutes: number | null;
    lateMinutes: number;
    overtimeMinutes: number;
}