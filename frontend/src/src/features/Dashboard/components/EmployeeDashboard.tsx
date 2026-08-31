import { useEffect, useState } from "react";
import type { EmployeeDashboardResponse } from "../types/EmployeeDashboardResponse";
import { LeaveBalanceCards } from "../../Leaves/components/LeaveBalanceCards";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { useAuthStore } from "../../../store/useAuthStore";
import axios from "axios";
import { Link } from "react-router-dom";
import { notifications } from "@mantine/notifications";
import {
    Alert,
    Avatar,
    Badge,
    Button,
    Card,
    Center,
    Group,
    Loader,
    Paper,
    Stack,
    Table,
    Text,
    ThemeIcon,
    Title
} from "@mantine/core";
import {
    IconClock,
    IconClockCheck,
    IconRefresh,
} from "@tabler/icons-react";

export function EmployeeDashboard() {
    const user = useAuthStore((state) => state.user);
    const [dashboardData, setDashboardData] = useState<EmployeeDashboardResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [isClocking, setIsClocking] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [currentTime, setCurrentTime] = useState(new Date());

    useEffect(() => {
        const timer = setInterval(() => setCurrentTime(new Date()), 1000);
        return () => clearInterval(timer);
    }, []);

    const fetchDashboard = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<EmployeeDashboardResponse>(
                API_ROUTES.DASHBOARD.GET_EMPLOYEE
            );
            setDashboardData(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : null;
            setError(message ?? "Unable to load dashboard summary.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchDashboard();
    }, []);

    // Quick Clock-In Action
    const handleClockIn = async () => {
        setIsClocking(true);
        try {
            await apiClient.post(API_ROUTES.ATTENDANCES.CLOCK_IN);
            notifications.show({
                title: "Clocked In",
                message: "You have successfully clocked in for today!",
                color: "green"
            });
            await fetchDashboard();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : "Clock in failed.";
            notifications.show({ title: "Clock In Error", message, color: "red" });
        } finally {
            setIsClocking(false);
        }
    };

    // Quick Clock-Out Action
    const handleClockOut = async () => {
        setIsClocking(true);
        try {
            await apiClient.post(API_ROUTES.ATTENDANCES.CLOCK_OUT);
            notifications.show({
                title: "Clocked Out",
                message: "You have successfully clocked out for today!",
                color: "blue"
            });
            await fetchDashboard();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : "Clock out failed.";
            notifications.show({ title: "Clock Out Error", message, color: "red" });
        } finally {
            setIsClocking(false);
        }
    };

    const renderStatusBadge = (status: number) => {
        switch (status) {
            case 1: return <Badge color="yellow" variant="light">Pending</Badge>;
            case 2: return <Badge color="green" variant="light">Approved</Badge>;
            case 3: return <Badge color="red" variant="light">Rejected</Badge>;
            case 4: return <Badge color="gray" variant="light">Cancelled</Badge>;
            default: return <Badge color="gray" variant="light">Unknown</Badge>;
        }
    };

    if (isLoading) {
        return (
            <Center py="xl" className="min-h-[60vh]">
                <Stack align="center" gap="md">
                    <Loader size="lg" color="indigo" />
                    <Text size="sm" c="dimmed">Loading your dashboard...</Text>
                </Stack>
            </Center>
        );
    }

    const todayAttendance = dashboardData?.todayAttendance;
    const isClockedIn = Boolean(todayAttendance?.clockIn);
    const isClockedOut = Boolean(todayAttendance?.clockOut);
    const isOnLeave = todayAttendance?.status === "OnLeave";

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header Welcome */}
                <Paper
                    radius="lg"
                    p="xl"
                    style={{
                        background: "linear-gradient(135deg, #3730a3 0%, #1e1b4b 100%)",
                        border: "1px solid rgba(255, 255, 255, 0.1)",
                        boxShadow: "0 10px 25px -5px rgba(0, 0, 0, 0.3)"
                    }}
                >
                    <Group justify="space-between" align="center" wrap="wrap">
                        <Group gap="lg">
                            <Avatar color="white" size={64} radius="xl" style={{ backgroundColor: "rgba(255,255,255,0.15)", color: "#ffffff" }}>
                                <Text size="xl" fw={700} c="white">
                                    {user ? `${user.firstName[0]}${user.lastName[0]}` : "U"}
                                </Text>
                            </Avatar>

                            <div>
                                <Title order={2} c="white" fw={700} mb={2}>
                                    Welcome back, {user?.firstName}! 👋
                                </Title>
                                <Text size="sm" c="indigo.1">
                                    {new Date().toLocaleDateString("en-US", {
                                        weekday: "long",
                                        year: "numeric",
                                        month: "long",
                                        day: "numeric"
                                    })}
                                </Text>
                            </div>
                        </Group>

                        <Group gap="sm">
                            <Badge size="lg" variant="light" color="indigo" radius="sm" leftSection={<IconClock size={15} />}>
                                {currentTime.toLocaleTimeString()}
                            </Badge>
                            <Badge size="lg" variant="filled" color="indigo" radius="sm">
                                {user?.email}
                            </Badge>
                        </Group>
                    </Group>
                </Paper>

                {error && (
                    <Alert color="red" title="Unable to load dashboard">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchDashboard}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                {/* Today's Attendance Widget & Quick Action */}
                <Card padding="lg" radius="md" withBorder shadow="sm">
                    <Group justify="space-between" align="center">
                        <div>
                            <Group gap="xs" mb={4}>
                                <ThemeIcon color="indigo" variant="light" radius="md">
                                    <IconClock size={18} />
                                </ThemeIcon>
                                <Title order={3}>Today's Attendance</Title>
                                <Badge color={isOnLeave ? "red" : isClockedOut ? "blue" : isClockedIn ? "green" : "yellow"} variant="light">
                                    {isOnLeave ? "On Leave" : isClockedOut ? "Completed Day" : isClockedIn ? "Clocked In" : "Not Clocked In Yet"}
                                </Badge>
                            </Group>
                            <Text size="sm" c="dimmed">
                                {isOnLeave ? "You are on leave today." : todayAttendance
                                    ? `Clocked In at ${todayAttendance.clockIn}${todayAttendance.clockOut ? ` • Clocked Out at ${todayAttendance.clockOut}` : ""}`
                                    : "You haven't clocked in today."}
                            </Text>
                        </div>

                        <Group gap="sm">
                            {!isClockedIn && (
                                <Button
                                    color="green"
                                    leftSection={<IconClockCheck size={18} />}
                                    loading={isClocking}
                                    onClick={handleClockIn}
                                >
                                    Clock In Now
                                </Button>
                            )}

                            {isClockedIn && !isClockedOut && !isOnLeave && (
                                <Button
                                    color="blue"
                                    leftSection={<IconClockCheck size={18} />}
                                    loading={isClocking}
                                    onClick={handleClockOut}
                                >
                                    Clock Out Now
                                </Button>
                            )}

                            <Button component={Link} to="/attendances" variant="light" color="indigo">
                                View Attendance Logs
                            </Button>
                        </Group>
                    </Group>
                </Card>

                {/* Leave Balances Grid */}
                <div>
                    <Group justify="space-between" align="center" mb="md">
                        <Title order={3}>My Leave Balances</Title>
                        <Button component={Link} to="/leaves/my" variant="subtle" size="xs">
                            View All Leave Details ➔
                        </Button>
                    </Group>
                    <LeaveBalanceCards balances={dashboardData?.leaveBalances ?? []} />
                </div>

                {/* Recent Leave Requests Tracking */}
                <div>
                    <Title order={3} mb="md">Recent Leave Applications</Title>
                    <Card padding="md" radius="md" withBorder>
                        {dashboardData?.recentRequests && dashboardData.recentRequests.length > 0 ? (
                            <Table highlightOnHover verticalSpacing="sm">
                                <Table.Thead>
                                    <Table.Tr>
                                        <Table.Th>Leave Type</Table.Th>
                                        <Table.Th>Start Date</Table.Th>
                                        <Table.Th>End Date</Table.Th>
                                        <Table.Th>Status</Table.Th>
                                    </Table.Tr>
                                </Table.Thead>
                                <Table.Tbody>
                                    {dashboardData.recentRequests.map((req) => (
                                        <Table.Tr key={req.id}>
                                            <Table.Td>
                                                <Badge color="indigo" variant="light">{req.typeName}</Badge>
                                            </Table.Td>
                                            <Table.Td>{req.startDate}</Table.Td>
                                            <Table.Td>{req.endDate}</Table.Td>
                                            <Table.Td>{renderStatusBadge(req.status)}</Table.Td>
                                        </Table.Tr>
                                    ))}
                                </Table.Tbody>
                            </Table>
                        ) : (
                            <Text size="sm" c="dimmed" ta="center" py="md">
                                No recent leave applications found.
                            </Text>
                        )}
                    </Card>
                </div>
            </Stack>
        </main>
    );
}