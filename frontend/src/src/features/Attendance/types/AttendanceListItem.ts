export interface AttendanceListItem {
    id: number;
    date: Date;
    clockIn: string;
    clockOut: string | null;
    status: string;
    totalMinutes: number | number;
    lateMinutes: number;
    overtimeMinutes: number;
}