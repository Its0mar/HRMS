import {
  Button,
  Group,
  NumberInput,
  Stack,
  Switch,
  TextInput,
} from "@mantine/core";
import type { UseFormReturnType } from "@mantine/form";
import type {  WorkScheduleDetails } from "../types/WorkScheduleDetails";
import { WorkScheduleDaysEditor } from "./WorkScheduleDaysEditor";

interface WorkScheduleFormProps {
  form: UseFormReturnType<WorkScheduleDetails>;
  onSubmit: (values: WorkScheduleDetails) => void | Promise<void>;
  isSubmitting: boolean;
  submitLabel: string;
  onCancel: () => void;
}

export function WorkScheduleForm({
  form,
  onSubmit,
  isSubmitting,
  submitLabel,
  onCancel,
}: WorkScheduleFormProps) {
  return (
    <form onSubmit={form.onSubmit(onSubmit)}>
      <Stack gap="lg">
        <TextInput
          label="Schedule name"
          placeholder="For example: Standard schedule"
          withAsterisk
          {...form.getInputProps("name")}
        />

        <NumberInput
          label="Grace period"
          description="Allowed late-arrival time in minutes"
          placeholder="For example: 10"
          min={0}
          max={1439}
          withAsterisk
          {...form.getInputProps("gracePeriodMinutes")}
        />

        <Switch
          label="Set as the default schedule"
          description="New employees can use this schedule by default"
          {...form.getInputProps("isDefault", {
            type: "checkbox",
          })}
        />

        <WorkScheduleDaysEditor form={form} />

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