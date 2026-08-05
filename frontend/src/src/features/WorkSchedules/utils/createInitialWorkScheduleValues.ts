import type {WorkScheduleDay} from "../types/WorkScheduleDay";
import type {WorkScheduleDetails} from "../types/WorkScheduleDetails";


function createWorkingDay(workDay: number): WorkScheduleDay {
  return {
    workDay,
    isWorkingDay: true,
    startTime: "09:00",
    endTime: "17:00",
    minimumMinutesPerDay: 480,
    breakDurationMinutes: 60,
  };
}

function createNonWorkingDay(workDay: number): WorkScheduleDay {
  return {
    workDay,
    isWorkingDay: false,
    startTime: null,
    endTime: null,
    minimumMinutesPerDay: null,
    breakDurationMinutes: 0,
  };
}

export function createInitialWorkScheduleValues(): WorkScheduleDetails {
  return {
    id: 0,
    name: "",
    gracePeriodMinutes: 0,
    isDefault: false,
    isActive: true,
    workScheduleDays: [
      createWorkingDay(1), // Monday
      createWorkingDay(2), // Tuesday
      createWorkingDay(3), // Wednesday
      createWorkingDay(4), // Thursday
      createWorkingDay(5), // Friday
      createNonWorkingDay(6), // Saturday
      createNonWorkingDay(7), // Sunday
    ],
  };
}