export interface MyLeaveRequestItem {
    id: number;
    typeName: string;
    startDate: string;
    endDate: string;
    totalDays: number;
    reason: string;
    status: number;
    rejectionReason: string | null;
}
