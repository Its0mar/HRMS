import {
  Button,
  Group,
  PasswordInput,
  Select,
  Stack,
  TextInput,
} from "@mantine/core";
import type { UseFormReturnType } from "@mantine/form";

import type { CreateAccessFormValues } from "../types/CreateAccessFormValues";
import type { RoleSelectOption } from "../types/RoleSelectOption";

interface EmployeeAccessFormProps {
  form: UseFormReturnType<CreateAccessFormValues>;
  roleOptions: RoleSelectOption[];
  isSubmitting: boolean;
  isLoadingRoles: boolean;
  submitLabel: string;
  showPasswordFields: boolean;
  onSubmit: (
    values: CreateAccessFormValues,
  ) => void | Promise<void>;
  onCancel: () => void;
}

export function EmployeeAccessForm({
  form,
  roleOptions,
  isSubmitting,
  isLoadingRoles,
  submitLabel,
  showPasswordFields,
  onSubmit,
  onCancel,
}: EmployeeAccessFormProps) {
  return (
    <form onSubmit={form.onSubmit(onSubmit)}>
      <Stack gap="md">
        <TextInput
          label="Username"
          withAsterisk
          disabled={isSubmitting}
          {...form.getInputProps("username")}
        />

        <Select
          label="Role"
          data={roleOptions}
          searchable
          withAsterisk
          disabled={isSubmitting || isLoadingRoles}
          {...form.getInputProps("roleId")}
        />

        {showPasswordFields && (
          <>
            <PasswordInput
              label="Temporary password"
              withAsterisk
              disabled={isSubmitting}
              {...form.getInputProps("password")}
            />

            <PasswordInput
              label="Confirm password"
              withAsterisk
              disabled={isSubmitting}
              {...form.getInputProps("confirmPassword")}
            />
          </>
        )}

        <Group justify="flex-end">
          <Button
            type="button"
            variant="default"
            onClick={onCancel}
            disabled={isSubmitting}
          >
            Cancel
          </Button>

          <Button type="submit" loading={isSubmitting}>
            {submitLabel}
          </Button>
        </Group>
      </Stack>
    </form>
  );
}