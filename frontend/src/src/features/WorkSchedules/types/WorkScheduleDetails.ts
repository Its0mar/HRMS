import type { WorkScheduleDay } from "./WorkScheduleDay";

export interface WorkScheduleDetails {
    id: number;
    name: string;
    gracePeriodMinutes: number;
    isDefault: boolean;
    isActive: boolean;
    workScheduleDays: WorkScheduleDay[];
}