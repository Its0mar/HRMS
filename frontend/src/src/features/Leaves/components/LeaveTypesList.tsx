import { useEffect, useState } from "react";
import type { LeaveTypeListItem } from "../types/LeaveTypeListItem";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Badge, Group, Stack, ThemeIcon, Title, Text, Alert, Button } from "@mantine/core";
import { IconBuildingCommunity, IconRefresh } from "@tabler/icons-react";
import { DataTable } from "../../../Common/DataTable/DataTable";
import { LeaveTypesListColumns } from "./LeaveTypesListColumns";



export function LeaveTypesList() {

    const [leaveTypesRecords, setLeaveTypesRecords] = useState<LeaveTypeListItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchCLeaveTypes = async () => {
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
            setError(message ?? "Unable to load organization attendance records.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchCLeaveTypes();
    }, []);



    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header */}
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconBuildingCommunity size={22} />
                            </ThemeIcon>

                            <Title order={1}>Company Leave Types</Title>
                        </Group>

                        <Text c="gray.4">
                            Monitor and manage leave types.
                        </Text>
                    </div>

                    <Badge size="lg" variant="light" color="indigo">
                        {leaveTypesRecords.length} Total Types
                    </Badge>
                </Group>


                {/* Error Banner */}
                {error && (
                    <Alert color="red" title="Unable to load attendance">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchCLeaveTypes}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                <DataTable
                    data={leaveTypesRecords}
                    columns={LeaveTypesListColumns}
                    getRowKey={(item) => item.id}
                    isLoading={isLoading}
                    minWidth={950}
                    emptyTitle="No leave types records found"
                    emptyDescription="No leave types for this organization."
                />


            </Stack>
        </main>
    )

}