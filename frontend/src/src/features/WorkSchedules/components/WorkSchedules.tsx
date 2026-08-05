import { useEffect, useState } from "react";
import type { WorkScheduleListItem } from "../types/WorkScheduleListItem";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { apiClient } from "../../../lib/apiClient";
import axios from "axios";
import { DataTable, type DataTableColumn } from "../../../Common/DataTable/DataTable";
import { Badge, Button, Group, Stack, ThemeIcon, Title, Text, Alert } from "@mantine/core";
import { IconCalendar, IconCalendarPlus, IconRefresh } from "@tabler/icons-react";
import { useDisclosure } from "@mantine/hooks";
import { CreateWorkScheduleModal } from "./CreateWorkScheduleModal";
import { UpdateWorkScheduleModal } from "./UpdateWorkScheduleModal";

export function WorkSchedules() {

    const [workSchedules, setWorkSchedules] = useState<WorkScheduleListItem[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [createOpened, createModal] = useDisclosure(false);
    const [selectedScheduleId, setSelectedScheduleId] = useState<number | null>(null);

    const handleEdit = (id: number) => {
        setSelectedScheduleId(id);
        // updateModal.open();
    };

    const fetchWorkSchedules = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<WorkScheduleListItem[]>(API_ROUTES.WORK_SCHEDULES.GET_ALL);
            setWorkSchedules(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "We could not load the work schedules.");
        } finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void fetchWorkSchedules();
    }, []);

    const columns : DataTableColumn<WorkScheduleListItem>[] = [
        {
            key : "number",
            header : "No.",
            width : 70,
            render : (_, index) => index + 1
        },

        {
            "key" : "name",
            "header" : "Name",
            "render" : (workSchedule) => workSchedule.name
        },

        {
            "key" : "gracePeriodMinutes",
            "header" : "Grace Period (Minutes)",
            "render" : (workSchedule) => workSchedule.gracePeriodMinutes
        },

        {
            "key" : "status",
            "header" : "Status",
            "render" : (workSchedule) =>             
            <Badge color={workSchedule.isActive ? "green" : "gray"}>
                {workSchedule.isActive ? "Active" : "Inactive"}
            </Badge>
        },

        {
            "key" : "default",
            "header" : "Default",
            "render" : (workSchedule) => workSchedule.isDefault ? <Badge color="indigo">default</Badge> : <Text c="dimmed">-</Text>
        },

                {
            key: "actions",
            header: "Actions",
            render: (workSchedule) => (
                <Group >
                    <Button
                        size="xs"
                        variant="light"
                        onClick={() => handleEdit(workSchedule.id)}
                    >
                        View/Edit
                    </Button>
                </Group>
            )
        }
    ];

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconCalendar size={30} />
                            </ThemeIcon>
                            <Title order={1}>Work Schedules</Title>
                        </Group>
                        <Text c="gray.4">
                            View and manage your organization work schedules.
                        </Text>
                    </div>
                        <Group>
                            <Badge size="lg" variant="light" color="indigo">
                                {workSchedules.length} total
                            </Badge>
                            <Button 
                                leftSection={<IconCalendarPlus size={16} />}
                                onClick={() => createModal.open()}>
                                New Work Schedule
                            </Button>
                        </Group>
                </Group>

                {error && (
                    <Alert
                        color="red"
                        title="Unable to load work schedules">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button
                                size="xs"
                                variant="light"
                                color="red"
                                leftSection={<IconRefresh size={15} />}
                                onClick={fetchWorkSchedules}
                            >
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                <CreateWorkScheduleModal
                    opened={createOpened}
                    onClose={createModal.close}
                    onCreated={() => {
                        void fetchWorkSchedules();
                    }}
                />

                <UpdateWorkScheduleModal
                    opened={selectedScheduleId !== null}
                    scheduleId={selectedScheduleId}
                    onClose={() => setSelectedScheduleId(null)}
                    onUpdated={() => {
                        void fetchWorkSchedules();
                        setSelectedScheduleId(null);    
                    }}
                />


                <DataTable
                    data={workSchedules}
                    columns={columns}
                    getRowKey={(workSchedule) => workSchedule.id}
                    isLoading={isLoading}
                    minWidth={1000}
                    emptyTitle="No work schedules yet"
                    emptyDescription="Work schedules will appear here once created."
                />
            </Stack>
        </main>

    )
}