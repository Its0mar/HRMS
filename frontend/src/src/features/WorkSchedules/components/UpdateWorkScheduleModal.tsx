import { useEffect, useState } from "react";
import { Alert, Center, Loader, Modal } from "@mantine/core";
import { useForm } from "@mantine/form";
import axios from "axios";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import type { WorkScheduleDetails } from "../types/WorkScheduleDetails";
import { createInitialWorkScheduleValues } from "../utils/createInitialWorkScheduleValues";
import { validateWorkSchedule } from "../utils/validateWorkSchedule";
import { WorkScheduleForm } from "./WorkScheduleForm";

interface UpdateWorkScheduleModalProps {
  opened: boolean;
  scheduleId: number | null;
  onClose: () => void;
  onUpdated: () => void;
}

function formatTime(time: string | null): string | null {
  if (!time) {
    return null;
  }

  return time.length === 5 ? `${time}:00` : time;
}

export function UpdateWorkScheduleModal({
  opened,
  scheduleId,
  onClose,
  onUpdated,
}: UpdateWorkScheduleModalProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [isUpdating, setIsUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<WorkScheduleDetails>({
    initialValues: createInitialWorkScheduleValues(),
    validate: validateWorkSchedule,
  });

  useEffect(() => {
    if (!opened || scheduleId === null) {
      return;
    }

    let cancelled = false;

    const fetchSchedule = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const response = await apiClient.get<WorkScheduleDetails>(
          API_ROUTES.WORK_SCHEDULES.GET_BY_ID(scheduleId),
        );

        if (cancelled) {
          return;
        }

        const schedule = response.data;

        form.setValues({
          name: schedule.name,
          gracePeriodMinutes: schedule.gracePeriodMinutes,
          isDefault: schedule.isDefault,
          workScheduleDays: schedule.workScheduleDays
            .map((day) => ({
              workDay: Number(day.workDay),
              isWorkingDay: day.isWorkingDay,
              startTime: day.startTime?.slice(0, 5) ?? null,
              endTime: day.endTime?.slice(0, 5) ?? null,
              minimumMinutesPerDay:
                day.minimumMinutesPerDay,
              breakDurationMinutes:
                day.breakDurationMinutes,
            }))
            .sort((first, second) => first.workDay - second.workDay),
        });

        form.clearErrors();
      } catch (requestError) {
        if (cancelled) {
          return;
        }

        const message = axios.isAxiosError(requestError)
          ? requestError.response?.data?.errors?.[0]?.description
          : null;

        setError(message ?? "Could not load the work schedule.");
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void fetchSchedule();

    return () => {
      cancelled = true;
    };
  }, [opened, scheduleId]);

  const handleClose = () => {
    form.setValues(createInitialWorkScheduleValues());
    form.clearErrors();
    setError(null);
    onClose();
  };

  const handleSubmit = async (values: WorkScheduleDetails) => {
    if (scheduleId === null) {
      return;
    }

    setIsUpdating(true);
    setError(null);

    try {
      await apiClient.put(
        API_ROUTES.WORK_SCHEDULES.UPDATE,
        {
          id: scheduleId,
          name: values.name.trim(),
          gracePeriodMinutes: values.gracePeriodMinutes,
          isDefault: values.isDefault,
          workScheduleDays: values.workScheduleDays.map((day) => ({
            workDay: day.workDay,
            isWorkingDay: day.isWorkingDay,

            startTime: day.isWorkingDay
              ? formatTime(day.startTime)
              : null,

            endTime: day.isWorkingDay
              ? formatTime(day.endTime)
              : null,

            minimumMinutesPerDay:
              day.isWorkingDay &&
              day.minimumMinutesPerDay !== null
                ? Number(day.minimumMinutesPerDay)
                : null,

            breakDurationMinutes: day.isWorkingDay
              ? Number(day.breakDurationMinutes || 0)
              : 0,
          })),
        },
      );

      onUpdated();
      handleClose();
    } catch (requestError) {
      const message = axios.isAxiosError(requestError)
        ? requestError.response?.data?.errors?.[0]?.description
        : null;

      setError(message ?? "Could not update the work schedule.");
    } finally {
      setIsUpdating(false);
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={handleClose}
      title="Update work schedule"
      size="xl"
      centered
      closeOnClickOutside={!isUpdating}
      closeOnEscape={!isUpdating}
    >
      {error && (
        <Alert color="red" title="Update failed" mb="md">
          {error}
        </Alert>
      )}

      {isLoading ? (
        <Center py="xl">
          <Loader />
        </Center>
      ) : (
        <WorkScheduleForm
          form={form}
          onSubmit={handleSubmit}
          isSubmitting={isUpdating}
          submitLabel="Save changes"
          onCancel={handleClose}
        />
      )}
    </Modal>
  );
}