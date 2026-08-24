import { useEffect, useState } from "react";
import type { AttendanceCorrectionListItem } from "../types/AttendanceCorrectionListItem";
import { Badge, Group, Stack, ThemeIcon, Title, Text, Alert, Button } from "@mantine/core";
import { DataTable } from "../../../Common/DataTable/DataTable";
import {  GetCompanyAttendanceCorrectionsColumns } from "./GetCompanyAttendanceCorrectionsColumns";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { IconCalendar, IconRefresh } from "@tabler/icons-react";

export function CompanyAttendanceCorrectionsList() {

    const [attendanceCorrections, setAttendanceCorrections] = useState<AttendanceCorrectionListItem[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const fetchattendanceCorrections = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const params: Record<string, string> = {};
            params.status = "1";

            const response = await apiClient.get<AttendanceCorrectionListItem[]>(API_ROUTES.ATTENDANCES.CORRECTIONS.GET_ALL,
                {
                    params
                }
            );
            setAttendanceCorrections(response.data);
        }
        catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "We could not load the attendances.");
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void fetchattendanceCorrections();
    }, []);


    async function handleApproveOrReject(id: number, action: "approve" | "reject") {
        try {
            await apiClient.post(API_ROUTES.ATTENDANCES.CORRECTIONS.Approve_Reject, {
                id: id,
                approve: action === "approve"
            });

            await fetchattendanceCorrections();
        } catch (err) {
            console.error("Error processing correction:", err);
        }
    }


    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header */}
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconCalendar size={22} />
                            </ThemeIcon>

                            <Title order={1}>Company Attendance Corrections Requests</Title>
                        </Group>

                        <Text c="gray.4">
                            Monitor and manage company attendance corrections requests across all departments.
                        </Text>
                    </div>

                    <Badge size="lg" variant="light" color="indigo">
                        {attendanceCorrections.length} Total Requests
                    </Badge>
                </Group>

                {/* Error Banner */}
                {error && (
                    <Alert color="red" title="Unable to load attendance">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchattendanceCorrections}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                <DataTable
                    data={attendanceCorrections}
                    columns={GetCompanyAttendanceCorrectionsColumns(handleApproveOrReject)}
                    getRowKey={(ac) => ac.id}
                    isLoading={isLoading}
                    minWidth={1000}
                    emptyTitle="No new attendance correction requests yet"
                    emptyDescription="attendances correction requests will appear here once created."
                />
            </Stack>
        </main>
    );
}