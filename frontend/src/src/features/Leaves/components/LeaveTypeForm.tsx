import type { UseFormReturnType } from "@mantine/form";
import type { LeaveTypeFormValues } from "../types/LeaveTypeFormValues";
import { Stack, TextInput, Group, Checkbox, Button, NumberInput } from "@mantine/core";

interface LeaveTypeFormProps {
    form: UseFormReturnType<LeaveTypeFormValues>;
    isSubmitting: boolean;
    submitLabel: string;
    onSubmit: (
        values: LeaveTypeFormValues,
    ) => void | Promise<void>;
    onCancel: () => void;
}


export function LeaveTypeForm({
    form,
    isSubmitting,
    submitLabel,
    onSubmit,
    onCancel }: LeaveTypeFormProps) {

    return (
        <form onSubmit={form.onSubmit(onSubmit)}>
            <Stack gap="lg">
                <TextInput
                    label="Leave Type name"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("name")}
                />

                <TextInput
                    label="Leave Type code"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("code")}
                />


                <NumberInput
                    label="default days per year"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("defaultDaysPerYear")}
                />

                <div>
                    <Group justify="space-between" mb="sm">
                        <Checkbox
                            checked={form.values.isPaid}
                            onChange={(event) => form.setFieldValue("isPaid", event.currentTarget.checked)}
                            label="Paid"
                        />

                        <Checkbox
                            checked={form.values.requiresApproval}
                            onChange={(event) => form.setFieldValue("requiresApproval", event.currentTarget.checked)}
                            label="Requires Approval"
                        />

                    </Group>
                </div>

                <Group justify="flex-end">
                    <Button
                        type="button"
                        variant="default"
                        onClick={onCancel}
                        disabled={isSubmitting}
                    >
                        Cancel
                    </Button>

                    <Button
                        type="submit"
                        loading={isSubmitting}
                    >
                        {submitLabel}
                    </Button>
                </Group>

            </Stack>
        </form>
    )
}