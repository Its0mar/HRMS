import { useForm } from "@mantine/form";
import { useState } from "react";
import type { WorkScheduleDetails } from "../types/WorkScheduleDetails";
import { createInitialWorkScheduleValues } from "../utils/createInitialWorkScheduleValues";
import { validateWorkSchedule } from "../utils/validateWorkSchedule";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { apiClient } from "../../../lib/apiClient";
import axios from "axios";
import { Alert, Modal } from "@mantine/core";
import { WorkScheduleForm } from "./WorkScheduleForm";

interface CreateWorkScheduleModalProps {
    opened: boolean;
    onClose: () => void;
    onCreated: () => void;
}

function formatTime(time: string | null): string | null {
    if (!time) {
        return null;
    }

    return time.length === 5 ? `${time}:00` : time;
}

export function CreateWorkScheduleModal({ opened, onClose, onCreated }: CreateWorkScheduleModalProps) {

    const [isCreating, setIsCreating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<WorkScheduleDetails>({
        initialValues: createInitialWorkScheduleValues(),
        validate: validateWorkSchedule,
    });

    const handleClose = () => {
        form.setValues(createInitialWorkScheduleValues());
        form.clearErrors();
        setError(null);
        onClose();
    };

    const handleSubmit = async (values: WorkScheduleDetails) => {
        setIsCreating(true);
        setError(null);

        try {
            await apiClient.post(API_ROUTES.WORK_SCHEDULES.CREATE, {
                name: values.name.trim(),
                gracePeriodMinutes: values.gracePeriodMinutes,
                isDefault: values.isDefault,

                workScheduleDay: values.workScheduleDays.map((day) => ({
                    workDay: day.workDay,
                    isWorkingDay: day.isWorkingDay,

                    startTime: day.isWorkingDay
                        ? formatTime(day.startTime)
                        : null,

                    endTime: day.isWorkingDay
                        ? formatTime(day.endTime)
                        : null,

                    minimumMinutesPerDay:
                        day.isWorkingDay && day.minimumMinutesPerDay !== null
                            ? Number(day.minimumMinutesPerDay)
                            : null,

                    breakDurationMinutes: day.isWorkingDay
                        ? Number(day.breakDurationMinutes || 0)
                        : 0,
                })),
            });

            onCreated();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "Could not create the work schedule.");
        } finally {
            setIsCreating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Create work schedule"
            size="xl"
            centered
        >
            {error && (
                <Alert color="red" title="Creation failed" mb="md">
                    {error}
                </Alert>
            )}

            <WorkScheduleForm
                form={form}
                onSubmit={handleSubmit}
                isSubmitting={isCreating}
                submitLabel="Create schedule"
                onCancel={handleClose}
            />
        </Modal>
    );
}