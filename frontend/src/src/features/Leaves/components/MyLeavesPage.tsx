import { useEffect, useState } from "react";
import type { MyLeaveBalanceItem } from "../types/MyLeaveBalanceItem";
import type { MyLeaveRequestItem } from "../types/MyLeaveRequestItem";
import { LeaveBalanceCards } from "./LeaveBalanceCards";
import { ApplyLeaveModal } from "./ApplyLeaveModal";
import { DataTable, type DataTableColumn } from "../../../Common/DataTable/DataTable";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { useDisclosure } from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import { Alert, Badge, Button, Group, Stack, Text, ThemeIcon, Title } from "@mantine/core";
import { IconCalendarEvent, IconPlus, IconRefresh } from "@tabler/icons-react";

export function MyLeavesPage() {
    const [balances, setBalances] = useState<MyLeaveBalanceItem[]>([]);
    const [requests, setRequests] = useState<MyLeaveRequestItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [applyOpened, applyModal] = useDisclosure(false);

    const currentYear = new Date().getFullYear();

    const fetchDashboardData = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const [balancesRes, requestsRes] = await Promise.all([
                apiClient.get<MyLeaveBalanceItem[]>(API_ROUTES.LEAVES.GET_MY_BALANCES, {
                    params: { year: currentYear }
                }),
                apiClient.get<MyLeaveRequestItem[]>(API_ROUTES.LEAVES.GET_MY_REQUESTS)
            ]);

            setBalances(balancesRes.data);
            setRequests(requestsRes.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "Unable to load leave dashboard data.");
        } finally {
            setIsLoading(false);
        }
    };

    const handleCancel = async (id: number) => {
        try {
            await apiClient.put(API_ROUTES.LEAVES.CANCEL_REQUEST(id));
            notifications.show({
                title: "Request Cancelled",
                message: "Your pending leave request has been cancelled and days restored.",
                color: "green"
            });
            await fetchDashboardData();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : "Unable to cancel leave request.";
            notifications.show({ title: "Cancellation Error", message, color: "red" });
        }
    };

    useEffect(() => {
        void fetchDashboardData();
    }, []);

    const handleSubmitted = () => {
        notifications.show({
            title: "Application Submitted",
            message: "Your leave request has been submitted for manager approval.",
            color: "green"
        });
        void fetchDashboardData();
    };

    const renderStatusBadge = (status: number) => {
        switch (status) {
            case 1:
                return <Badge color="yellow" variant="light">Pending</Badge>;
            case 2:
                return <Badge color="green" variant="light">Approved</Badge>;
            case 3:
                return <Badge color="red" variant="light">Rejected</Badge>;
            case 4:
                return <Badge color="gray" variant="light">Cancelled</Badge>;
            default:
                return <Badge color="gray" variant="light">Unknown</Badge>;
        }
    };

    const columns: DataTableColumn<MyLeaveRequestItem>[] = [
        { key: "no", header: "No.", width: 60, render: (_, idx) => idx + 1 },
        { key: "type", header: "Leave Type", render: (item) => <Badge color="indigo" variant="light">{item.typeName}</Badge> },
        { key: "dates", header: "Date Range", render: (item) => `${item.startDate} ➔ ${item.endDate}` },
        { key: "duration", header: "Duration", render: (item) => `${item.totalDays} days` },
        { key: "reason", header: "Reason", render: (item) => item.reason },
        {
            key: "status",
            header: "Status",
            render: (item) => renderStatusBadge(item.status)
        },
        {
            key: "actions",
            header: "Actions",
            render: (item) => (
                item.status === 1 ? (
                    <Button size="xs" variant="light" color="red" onClick={() => handleCancel(item.id)}>
                        Cancel
                    </Button>
                ) : (
                    <Text size="xs" c="dimmed">-</Text>
                )
            )
        }
    ];

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header */}
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconCalendarEvent size={22} />
                            </ThemeIcon>
                            <Title order={1}>My Leave Dashboard</Title>
                        </Group>
                        <Text c="gray.4">Track your available leave balances and apply for vacations.</Text>
                    </div>

                    <Button leftSection={<IconPlus size={16} />} onClick={applyModal.open}>
                        Apply for Leave
                    </Button>
                </Group>

                {error && (
                    <Alert color="red" title="Unable to load dashboard">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchDashboardData}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                {/* 1. Leave Balance Cards */}
                <div>
                    <Title order={3} mb="md">Leave Balances ({currentYear})</Title>
                    <LeaveBalanceCards balances={balances} />
                </div>

                {/* 2. My Leave Requests History */}
                <div>
                    <Title order={3} mb="md">My Leave Requests</Title>
                    <DataTable
                        data={requests}
                        columns={columns}
                        getRowKey={(item) => item.id}
                        isLoading={isLoading}
                        emptyTitle="No leave applications yet"
                        emptyDescription="Click 'Apply for Leave' to submit your first request."
                    />
                </div>

                {/* 3. Apply Modal */}
                <ApplyLeaveModal
                    opened={applyOpened}
                    balances={balances}
                    onClose={applyModal.close}
                    onSubmitted={handleSubmitted}
                />
            </Stack>
        </main>
    );
}