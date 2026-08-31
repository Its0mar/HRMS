import type { MyLeaveBalanceItem } from "../../Leaves/types/MyLeaveBalanceItem";

export interface TodayAttendanceDto {
    id: number;
    date: string;
    clockIn: string;
    clockOut: string | null;
    status: string;
    totalMinutes: number | null;
    lateMinutes: number;
}

export interface RecentLeaveRequestDto {
    id: number;
    typeName: string;
    startDate: string;
    endDate: string;
    status: number;
}

export interface EmployeeDashboardResponse {
    todayAttendance: TodayAttendanceDto | null;
    leaveBalances: MyLeaveBalanceItem[];
    recentRequests: RecentLeaveRequestDto[];
}
