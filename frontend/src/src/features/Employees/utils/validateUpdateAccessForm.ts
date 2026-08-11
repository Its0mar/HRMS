import type { FormErrors } from "@mantine/form";
import type { CreateAccessFormValues } from "../types/CreateAccessFormValues";

export function validateUpdateAccessForm(
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

  return errors;
}