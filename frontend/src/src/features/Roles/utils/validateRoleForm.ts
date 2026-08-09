import type { FormErrors } from "@mantine/form";
import type { RoleFormValues } from "../types/RoleFormValues";


export function validateRoleForm(
  values: RoleFormValues,
): FormErrors {
  const errors: FormErrors = {};
  const roleName = values.name.trim();

  if (roleName.length < 3) {
    errors.name =
      "Role name must contain at least 3 characters";
  }

  if (roleName.length > 30) {
    errors.name =
      "Role name cannot exceed 30 characters";
  }

  if (values.permissionIds.length === 0) {
    errors.permissionIds =
      "Select at least one permission";
  }

  return errors;
}