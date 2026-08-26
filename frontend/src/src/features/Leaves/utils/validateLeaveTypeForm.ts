import type { FormErrors } from "@mantine/form";
import type { LeaveTypeFormValues } from "../types/LeaveTypeFormValues";

export function validateLeaveTypeForm(values: LeaveTypeFormValues): FormErrors {
    const errors: FormErrors = {};

    if (!values.name.trim()) {
        errors.name = "Leave Type Name is required.";
    } else if (values.name.trim().length < 2) {
        errors.name = "Name must contain at least 2 characters.";
    } else if (values.name.trim().length > 100) {
        errors.name = "Name cannot exceed 100 characters.";
    }

    if (!values.code.trim()) {
        errors.code = "Leave Type Code is required.";
    } else if (values.code.trim().length < 2) {
        errors.code = "Code must contain at least 2 characters.";
    } else if (values.code.trim().length > 20) {
        errors.code = "Code cannot exceed 20 characters.";
    }

    if (values.defaultDaysPerYear === undefined || values.defaultDaysPerYear === null) {
        errors.defaultDaysPerYear = "Default days per year is required.";
    } else if (values.defaultDaysPerYear < 0) {
        errors.defaultDaysPerYear = "Default days cannot be negative.";
    } else if (values.defaultDaysPerYear > 365) {
        errors.defaultDaysPerYear = "Default days cannot exceed 365 days.";
    }

    return errors;
}