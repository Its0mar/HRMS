import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import type { AssignEmployeeScheduleFormValues } from "../types/AssignEmployeeScheduleFormValues";
import { Alert, Button, Group, Loader, Modal, Select, Stack, Text } from "@mantine/core";
import type { WorkScheduleOption } from "../../WorkSchedules/types/WorkScheduleOption";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";

interface AssignWorkScheduleModalProps {
    opened: boolean;
    employeeId: number | null;
    employeeName: string | null;
    onClose: () => void;
    onCreated: () => void;
}

export function AssignWorkScheduleModal({ opened, employeeId, employeeName, onClose, onCreated }: AssignWorkScheduleModalProps) {
    const [isAssigning, setIsAssigning] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isLoadingOptions, setIsLoadingOptions] = useState(false);
    const [workScheduleOptions, setWorkScheduleOptions] = useState<Array<{ value: string; label: string }>>([]);

    const form = useForm<AssignEmployeeScheduleFormValues>({
        initialValues: {
            workScheduleId: null,
        },
        validate: {
            workScheduleId: (value) => value ? null : 'Work schedule is required',
        }
    });

    const handleClose = () => {
        form.setValues({
            workScheduleId: null,
        });
        form.clearErrors();
        setError(null);
        onClose();
    };

    const handleSubmit = async (
        values: AssignEmployeeScheduleFormValues,
    ) => {
        if (
            employeeId === null ||
            values.workScheduleId === null
        ) {
            return;
        }

        setIsAssigning(true);
        setError(null);

        try {
            await apiClient.post(
                API_ROUTES.WORK_SCHEDULES.ASSIGN_EMPLOYEE,
                {
                    employeeId,
                    workScheduleId: Number(values.workScheduleId),
                },
            );

            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(
                message ?? "Could not assign the work schedule.",
            );
        } finally {
            setIsAssigning(false);
        }
    };

    useEffect(() => {
        if (!opened) return;
        let cancelled = false;

        const loadWorkSchedules = async () => {
            setIsLoadingOptions(true);
            setError(null);

            try {
                const response = await apiClient.get<WorkScheduleOption[]>(API_ROUTES.WORK_SCHEDULES.GET_OPTIONS);
                setWorkScheduleOptions(response.data.map((item) => ({
                    value: item.id.toString(),
                    label: item.name,
                })));
            } catch (requestError) {
                if (!cancelled) {
                    const message = axios.isAxiosError(requestError)
                        ? requestError.response?.data?.errors?.[0]?.description
                        : null;
                    setError(message ?? "We could not load the work schedules.");
                }
            } finally {
                if (!cancelled) {
                    setIsLoadingOptions(false);
                }
            }
        };

        void loadWorkSchedules();

        return () => {
            cancelled = true;
        };
    }, [opened]);

    return (
        <Modal
            opened={opened}
            title="Assign Work Schedule"
            onClose={handleClose}
            size="md"
            closeOnClickOutside={!isAssigning}
            closeOnEscape={!isAssigning}
        >
            <Stack gap="md">
                {employeeName && (
                    <div>
                        <Text size="sm" c="dimmed">
                            Assigning a schedule to
                        </Text>

                        <Text fw={600}>
                            {employeeName}
                        </Text>
                    </div>
                )}

                {error && (
                    <Alert color="red" title="Unable to assign schedule">
                        {error}
                    </Alert>
                )}

                <form
                    onSubmit={form.onSubmit(() => {
                        handleSubmit(form.values);
                    })}
                >
                    <Stack gap="md">
                        <Select
                            label="Work schedule"
                            placeholder={
                                isLoadingOptions
                                    ? "Loading schedules..."
                                    : "Select a work schedule"
                            }
                            data={workScheduleOptions}
                            searchable
                            withAsterisk
                            disabled={isLoadingOptions || isAssigning}
                            nothingFoundMessage="No work schedules found"
                            rightSection={
                                isLoadingOptions
                                    ? <Loader size="xs" />
                                    : undefined
                            }
                            {...form.getInputProps("workScheduleId")}
                        />

                        <Group justify="flex-end">
                            <Button
                                type="button"
                                variant="default"
                                disabled={isAssigning}
                                onClick={handleClose}
                            >
                                Cancel
                            </Button>

                            <Button
                                type="submit"
                                loading={isAssigning}
                                disabled={isLoadingOptions}
                            >
                                Assign schedule
                            </Button>
                        </Group>
                    </Stack>
                </form>
            </Stack>

        </Modal>
    );
}