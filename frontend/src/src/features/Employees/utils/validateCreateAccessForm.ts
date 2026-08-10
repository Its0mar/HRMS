import type { FormErrors } from "@mantine/form";
import type { CreateAccessFormValues } from "../types/CreateAccessFormValues";

export function validateCreateAccessForm(
  values: CreateAccessFormValues,
): FormErrors {
  const errors: FormErrors = {};
  const username = values.username.trim();

  if (username.length < 3) {
    errors.username =
      "Username must contain at least 3 characters.";
  }

  if (username.length > 20) {
    errors.username =
      "Username cannot exceed 20 characters.";
  }

  if (!values.roleId) {
    errors.roleId = "Select a role.";
  }

  if (values.password.length < 8) {
    errors.password =
      "Password must contain at least 8 characters.";
  }

  if (values.password.length > 100) {
    errors.password =
      "Password cannot exceed 100 characters.";
  }

  if (!values.confirmPassword) {
    errors.confirmPassword =
      "Confirm your password.";
  } else if (
    values.password !== values.confirmPassword
  ) {
    errors.confirmPassword =
      "Passwords do not match.";
  }

  return errors;
}