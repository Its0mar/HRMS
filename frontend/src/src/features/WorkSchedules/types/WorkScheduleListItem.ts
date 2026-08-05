export interface WorkScheduleListItem {
    id: number;
    name: string;
    gracePeriodMinutes: number;
    isDefault: boolean;
    isActive: boolean;
}