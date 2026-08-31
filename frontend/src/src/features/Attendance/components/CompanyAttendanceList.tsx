import { useEffect, useState } from "react";
import type { CompanyAttendanceItem } from "../types/CompanyAttendanceItem";
import { CompanyAttendanceColumns } from "./CompanyAttendanceColumns";
import { DataTable } from "../../../Common/DataTable/DataTable";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import {
    Alert,
    Badge,
    Button,
    Card,
    Group,
    SimpleGrid,
    Stack,
    Text,
    TextInput,
    ThemeIcon,
    Title
} from "@mantine/core";
import {
    IconBuildingCommunity,
    IconSearch,
    IconRefresh,
    IconCheck,
    IconClockCheck,
    IconAlertCircle,
    IconCalendar
} from "@tabler/icons-react";

export function CompanyAttendanceList() {
    const [records, setRecords] = useState<CompanyAttendanceItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Filters
    const [selectedDate, setSelectedDate] = useState<string>(new Date().toLocaleDateString("en-CA")); // Today YYYY-MM-DD
    const [searchTerm, setSearchTerm] = useState("");

    const fetchCompanyAttendance = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const params: Record<string, string> = {};
            if (selectedDate) params.date = selectedDate;
            if (searchTerm.trim()) params.searchTerm = searchTerm.trim();

            const response = await apiClient.get<CompanyAttendanceItem[]>(
                API_ROUTES.ATTENDANCES.GET_ORGANIZATION,
                { params }
            );

            setRecords(response.data);
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
        void fetchCompanyAttendance();
    }, [selectedDate]);

    // Quick Stats
    const totalPresent = records.filter((r) => r.status === "Present").length;
    const totalLate = records.filter((r) => r.status === "Late").length;
    const totalAbsent = records.filter((r) => r.status === "Absent" || r.status === "OnLeave").length;

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

                            <Title order={1}>Company Attendance</Title>
                        </Group>

                        <Text c="gray.4">
                            Monitor and manage attendance logs across all departments.
                        </Text>
                    </div>

                    <Badge size="lg" variant="light" color="indigo">
                        {records.length} Total Logs
                    </Badge>
                </Group>

                {/* Quick Summary Cards */}
                <SimpleGrid cols={{ base: 1, sm: 3 }}>
                    <Card radius="md" padding="md" withBorder>
                        <Group justify="space-between">
                            <div>
                                <Text size="xs" c="dimmed" fw={700} tt="uppercase">Present Employees</Text>
                                <Text size="xl" fw={700} c="green.4">{totalPresent}</Text>
                            </div>
                            <ThemeIcon size="lg" radius="md" color="green" variant="light">
                                <IconCheck size={20} />
                            </ThemeIcon>
                        </Group>
                    </Card>

                    <Card radius="md" padding="md" withBorder>
                        <Group justify="space-between">
                            <div>
                                <Text size="xs" c="dimmed" fw={700} tt="uppercase">Late Arrivals</Text>
                                <Text size="xl" fw={700} c="yellow.4">{totalLate}</Text>
                            </div>
                            <ThemeIcon size="lg" radius="md" color="yellow" variant="light">
                                <IconClockCheck size={20} />
                            </ThemeIcon>
                        </Group>
                    </Card>

                    <Card radius="md" padding="md" withBorder>
                        <Group justify="space-between">
                            <div>
                                <Text size="xs" c="dimmed" fw={700} tt="uppercase">Absences</Text>
                                <Text size="xl" fw={700} c="red.4">{totalAbsent}</Text>
                            </div>
                            <ThemeIcon size="lg" radius="md" color="red" variant="light">
                                <IconAlertCircle size={20} />
                            </ThemeIcon>
                        </Group>
                    </Card>
                </SimpleGrid>

                {/* Filter Controls Bar */}
                <Card radius="md" padding="md" withBorder>
                    <Group justify="space-between">
                        <Group gap="md">
                            <TextInput
                                type="date"
                                label="Filter by Date"
                                value={selectedDate}
                                onChange={(e) => setSelectedDate(e.target.value)}
                                leftSection={<IconCalendar size={16} />}
                            />

                            <TextInput
                                label="Search Employee"
                                placeholder="Search by name..."
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                                onKeyDown={(e) => e.key === "Enter" && fetchCompanyAttendance()}
                                leftSection={<IconSearch size={16} />}
                            />
                        </Group>

                        <Group align="flex-end">
                            <Button
                                variant="light"
                                leftSection={<IconSearch size={16} />}
                                onClick={fetchCompanyAttendance}
                            >
                                Apply Filters
                            </Button>
                            <Button
                                variant="subtle"
                                color="gray"
                                onClick={() => {
                                    setSelectedDate(new Date().toLocaleDateString("en-CA"));
                                    setSearchTerm("");
                                }}
                            >
                                Reset
                            </Button>
                        </Group>
                    </Group>
                </Card>

                {/* Error Banner */}
                {error && (
                    <Alert color="red" title="Unable to load attendance">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchCompanyAttendance}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                {/* Company Attendance Data Table */}
                <DataTable
                    data={records}
                    columns={CompanyAttendanceColumns}
                    getRowKey={(item) => item.id}
                    isLoading={isLoading}
                    minWidth={950}
                    emptyTitle="No attendance records found"
                    emptyDescription="No attendance logs found matching the selected date or search filter."
                />
            </Stack>
        </main>
    );
}