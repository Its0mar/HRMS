import { useEffect, useState } from "react";
import type { LeaveTypeListItem } from "../types/LeaveTypeListItem";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Badge, Group, Stack, ThemeIcon, Title, Text, Alert, Button } from "@mantine/core";
import { IconBuildingCommunity, IconRefresh, IconPlus } from "@tabler/icons-react";
import { DataTable } from "../../../Common/DataTable/DataTable";
import { getLeaveTypesListColumns } from "./LeaveTypesListColumns";
import { useDisclosure } from "@mantine/hooks";
import { CreateLeaveTypeModal } from "./CreateLeaveTypeModal";
import { UpdateLeaveTypeModal } from "./UpdateLeaveTypeModal";

export function LeaveTypesList() {
    const [leaveTypesRecords, setLeaveTypesRecords] = useState<LeaveTypeListItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Modals & Edit selection
    const [createOpened, createModal] = useDisclosure(false);
    const [updateOpened, updateModal] = useDisclosure(false);
    const [selectedLeaveType, setSelectedLeaveType] = useState<LeaveTypeListItem | null>(null);

    const fetchLeaveTypes = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<LeaveTypeListItem[]>(
                API_ROUTES.LEAVES.GET_ALL
            );
            setLeaveTypesRecords(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "Unable to load leave types.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchLeaveTypes();
    }, []);

    const handleEdit = (item: LeaveTypeListItem) => {
        setSelectedLeaveType(item);
        updateModal.open();
    };

    const handleUpdateClose = () => {
        updateModal.close();
        setSelectedLeaveType(null);
    };

    const columns = getLeaveTypesListColumns(handleEdit);

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconBuildingCommunity size={22} />
                            </ThemeIcon>
                            <Title order={1}>Company Leave Types</Title>
                        </Group>
                        <Text c="gray.4">Monitor and manage company leave types.</Text>
                    </div>

                    <Group gap="md">
                        <Badge size="lg" variant="light" color="indigo">
                            {leaveTypesRecords.length} Total Types
                        </Badge>
                        <Button leftSection={<IconPlus size={16} />} onClick={createModal.open}>
                            New Leave Type
                        </Button>
                    </Group>
                </Group>

                {error && (
                    <Alert color="red" title="Unable to load leave types">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchLeaveTypes}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                <DataTable
                    data={leaveTypesRecords}
                    columns={columns}
                    getRowKey={(item) => item.id}
                    isLoading={isLoading}
                    minWidth={950}
                />

                <CreateLeaveTypeModal
                    opened={createOpened}
                    onClose={createModal.close}
                    onCreated={fetchLeaveTypes}
                />

                <UpdateLeaveTypeModal
                    opened={updateOpened}
                    leaveType={selectedLeaveType}
                    onClose={handleUpdateClose}
                    onUpdated={fetchLeaveTypes}
                />
            </Stack>
        </main>
    );
}