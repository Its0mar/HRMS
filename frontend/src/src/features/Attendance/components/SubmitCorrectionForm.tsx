import type { UseFormReturnType } from "@mantine/form";
import type { SubmitCorrectionFormValue } from "../types/SubmitCorrectionFormValue";
import { Button, Group, SimpleGrid, Stack, TextInput } from "@mantine/core";

interface SubmiCorrectionProps {
      form: UseFormReturnType<SubmitCorrectionFormValue>;
      isSubmitting: boolean;
      onSubmit: (
        values: SubmitCorrectionFormValue,
      ) => void | Promise<void>;
      onCancel: () => void;
}

export function SubmitCorrectionForm({form, isSubmitting, onSubmit, onCancel}: SubmiCorrectionProps)  {

  return (
    <form onSubmit={form.onSubmit(onSubmit)}>
      <Stack gap="xl">
        <Group>
          <SimpleGrid cols={2}>
            <TextInput
              type="datetime-local"
              label="Clock-in"
              placeholder="YYYY-MM-DD HH:mm:ss"
              withAsterisk
              {...form.getInputProps("requestedClockIn")}
            />
            <TextInput
              type="datetime-local"
              label="Clock-out"
              placeholder="YYYY-MM-DD HH:mm:ss"
              withAsterisk
              {...form.getInputProps("requestedClockOut")}
            />
          </SimpleGrid>
        </Group>

        <TextInput
          label="Reason"
          placeholder="Reason for correction"
          withAsterisk
          {...form.getInputProps("reason")}
        />

        <Group justify="flex-end" mt="lg">
          <Button type="button" variant="light" color="gray" onClick={onCancel} disabled={isSubmitting}>
            Cancel
          </Button>
          <Button type="submit" loading={isSubmitting}>
            Submit Request
          </Button>
        </Group>

      </Stack>
    </form>
  )
}