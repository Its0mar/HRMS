export interface LeaveTypeListItem {
    id: number;
    name: string;
    code: string;
    defaultDaysPerYear: number;
    isPaid: boolean;
    requiresApproval: boolean;
}