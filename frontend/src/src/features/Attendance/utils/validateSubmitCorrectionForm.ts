import type { FormErrors } from "@mantine/form";
import type { SubmitCorrectionFormValue } from "../types/SubmitCorrectionFormValue";

export function validateSubmitCorrectionForm(
    values: SubmitCorrectionFormValue
): FormErrors {
    const errors: FormErrors = {};

    if (!values.requestedClockIn) errors.requestedClockIn = "Clock-in is required.";
    if (!values.requestedClockOut) errors.requestedClockOut = "Clock-out is required.";

    if (values.requestedClockIn && values.requestedClockOut) {
        if (new Date(values.requestedClockOut) <= new Date(values.requestedClockIn)) {
            errors.requestedClockOut = "Clock-out time must be after Clock-in time.";
        }
    }

    if (!values.reason.trim()) {
        errors.reason = "A reason for the correction is required.";
    } else if (values.reason.trim().length < 3) {
        errors.reason = "Reason must contain at least 3 characters.";
    } else if (values.reason.trim().length > 300) {
        errors.reason = "Reason cannot exceed 300 characters.";
    }

    return errors;
}