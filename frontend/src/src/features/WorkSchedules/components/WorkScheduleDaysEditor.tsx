import type { UseFormReturnType } from "@mantine/form";
import type { WorkScheduleDetails } from "../types/WorkScheduleDetails";
import { Group, NumberInput, Paper, SimpleGrid, Stack, Switch, Text, TextInput } from "@mantine/core";
import type { WorkScheduleDay } from "../types/WorkScheduleDay";

interface WorkScheduleDaysEditorProps {
    form: UseFormReturnType<WorkScheduleDetails>;
}

export function WorkScheduleDaysEditor({
    form,
}: WorkScheduleDaysEditorProps) {

    const days = ["monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"];

    const handleWorkingDayChange = (
        index: number,
        checked: boolean
    ) => {
        form.setFieldValue(`workScheduleDays.${index}.isWorkingDay`, checked);
        if (!checked) {
            form.setFieldValue(
                `workScheduleDays.${index}.startTime`,
                null,
            );
            form.setFieldValue(
                `workScheduleDays.${index}.endTime`,
                null,
            );
            form.setFieldValue(
                `workScheduleDays.${index}.minimumMinutesPerDay`,
                null,
            );
            form.setFieldValue(
                `workScheduleDays.${index}.breakDurationMinutes`,
                0,
            );
        }
    }



    return (
        <Stack gap="sm">
            <Text fw={600}>Work Schedule Days</Text>

            {
                form.values.workScheduleDays.map((day: WorkScheduleDay, index: number) => (
                    <Paper key={day.workDay} withBorder p="md" radius="md" shadow="md">
                        <Group justify="space-between" mb="sm">
                            <Text>{days[day.workDay]}</Text>

                            <Switch
                                label="Working day"
                                checked={day.isWorkingDay}
                                onChange={(event) =>
                                    handleWorkingDayChange(
                                        index,
                                        event.currentTarget.checked,
                                    )
                                }
                            />
                        </Group>

                        <SimpleGrid cols={{ base: 1, sm: 4 }}>
                            <TextInput
                                type="time"
                                label="Start time"
                                disabled={!day.isWorkingDay}
                                {...form.getInputProps(
                                    `workScheduleDays.${index}.startTime`,
                                )}
                            />

                            <TextInput
                                type="time"
                                label="End time"
                                disabled={!day.isWorkingDay}
                                {...form.getInputProps(
                                    `workScheduleDays.${index}.endTime`,
                                )}
                            />

                            <NumberInput
                                label="Minimum minutes"
                                min={0}
                                disabled={!day.isWorkingDay}
                                {...form.getInputProps(
                                    `workScheduleDays.${index}.minimumMinutesPerDay`,
                                )}
                            />

                            <NumberInput
                                label="Break minutes"
                                min={0}
                                disabled={!day.isWorkingDay}
                                {...form.getInputProps(
                                    `workScheduleDays.${index}.breakDurationMinutes`,
                                )}
                            />
                        </SimpleGrid>
                    </Paper>
                ))
            }
        </Stack>
    );
}