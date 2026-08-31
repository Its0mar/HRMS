import type { FormErrors } from "@mantine/form";
import type { ApplyLeaveFormValues } from "../types/ApplyLeaveFormValues";

export function validateApplyLeaveForm(values: ApplyLeaveFormValues): FormErrors {
    const errors: FormErrors = {};

    if (!values.leaveTypeId) {
        errors.leaveTypeId = "Please select a leave type.";
    }

    if (!values.startDate) {
        errors.startDate = "Start date is required.";
    }

    if (!values.endDate) {
        errors.endDate = "End date is required.";
    }

    if (values.startDate && values.endDate) {
        const start = new Date(values.startDate);
        const end = new Date(values.endDate);
        if (start > end) {
            errors.endDate = "End date cannot be earlier than start date.";
        }
    }

    if (!values.reason.trim()) {
        errors.reason = "Reason is required.";
    } else if (values.reason.trim().length < 5) {
        errors.reason = "Reason must be at least 5 characters.";
    }

    return errors;
}
