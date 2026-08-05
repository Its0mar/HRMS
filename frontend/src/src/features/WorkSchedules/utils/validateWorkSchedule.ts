import type { FormErrors } from "@mantine/form";
import type { WorkScheduleDetails } from "../types/WorkScheduleDetails";

function timeToMinutes(time: string): number {
    const [hours, minutes] = time.split(":").map(Number);
    return hours * 60 + minutes;
}

export function validateWorkSchedule(
    values: WorkScheduleDetails
): FormErrors {

    const errors: FormErrors = {};

    if (values.name.trim().length < 3) {
        errors.name = "Name must contain at least 3 characters";
    }

    if (
        values.gracePeriodMinutes < 0 ||
        values.gracePeriodMinutes >= 1440
    ) {
        errors.gracePeriodMinutes =
            "Grace period must be between 0 and 1439 minutes";
    }

    if (!values.workScheduleDays.some((day) => day.isWorkingDay)) {
        errors.workScheduleDays =
            "At least one working day is required";
    }

    values.workScheduleDays.forEach((day, index) => {
        if (!day.isWorkingDay) {
            return;
        }

        if (!day.startTime) {
            errors[`workScheduleDays.${index}.startTime`] =
                "Start time is required";
        }

        if (!day.endTime) {
            errors[`workScheduleDays.${index}.endTime`] =
                "End time is required";
        }

        if (
            day.minimumMinutesPerDay === null ||
            day.minimumMinutesPerDay <= 0
        ) {
            errors[`workScheduleDays.${index}.minimumMinutesPerDay`] =
                "Minimum minutes must be greater than 0";
        }

        if (day.breakDurationMinutes < 0) {
            errors[`workScheduleDays.${index}.breakDurationMinutes`] =
                "Break duration cannot be negative";
        }

        if (day.startTime && day.endTime) {
            const start = timeToMinutes(day.startTime);
            const end = timeToMinutes(day.endTime);
            const shiftDuration = end - start;

            if (end <= start) {
                errors[`workScheduleDays.${index}.endTime`] =
                    "End time must be later than start time";
                return;
            }

            if (day.breakDurationMinutes >= shiftDuration) {
                errors[`workScheduleDays.${index}.breakDurationMinutes`] =
                    "Break must be shorter than the shift";
            }

            if (
                day.minimumMinutesPerDay !== null &&
                day.minimumMinutesPerDay >
                shiftDuration - day.breakDurationMinutes
            ) {
                errors[`workScheduleDays.${index}.minimumMinutesPerDay`] =
                    "Minimum minutes cannot exceed available working time";
            }
        }
    });

    return errors;
}