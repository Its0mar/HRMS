export interface MyLeaveBalanceItem {
    leaveTypeId: number;
    leaveTypeName: string;
    leaveTypeCode: string;
    isPaid: boolean;
    requiresApproval: boolean;
    year: number;
    totalEntitledDays: number;
    usedDays: number;
    pendingDays: number;
    remainingDays: number;
    isRecordedInDb: boolean;
}
