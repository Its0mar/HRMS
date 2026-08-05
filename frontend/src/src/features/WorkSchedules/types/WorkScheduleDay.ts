    export interface WorkScheduleDay {
        workDay: number,
        isWorkingDay: boolean,
        startTime: string | null,
        endTime: string | null,
        minimumMinutesPerDay: number | null,
        breakDurationMinutes: number
    }