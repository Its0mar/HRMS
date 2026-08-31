import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuthStore } from "../../../store/useAuthStore";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
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
    Progress,
    SimpleGrid,
    Stack,
    Table,
    Text,
    ThemeIcon,
    Title
} from "@mantine/core";
import {
    IconAlertCircle,
    IconClock,
    IconClockCheck,
    IconFileCheck,
    IconRefresh,
    IconUserCheck,
    IconUsers
} from "@tabler/icons-react";

interface AdminDashboardKpiDto {
    totalEmployees: number;
    presentToday: number;
    lateToday: number;
    onLeaveToday: number;
    pendingCorrectionsCount: number;
    pendingLeaveRequestsCount: number;
}

interface PendingLeaveRequestSummaryDto {
    id: number;
    employeeName: string;
    employeeNumber: string;
    leaveTypeName: string;
    startDate: string;
    endDate: string;
    totalDays: number;
    reason: string;
}

interface PendingCorrectionSummaryDto {
    id: number;
    employeeName: string;
    employeeNumber: string;
    reason: string;
}

interface AdminDashboardResponse {
    kpis: AdminDashboardKpiDto;
    pendingLeaves: PendingLeaveRequestSummaryDto[];
    pendingCorrections: PendingCorrectionSummaryDto[];
}

export function AdminDashboard() {
    const user = useAuthStore((state) => state.user);
    const [data, setData] = useState<AdminDashboardResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [currentTime, setCurrentTime] = useState(new Date());

    useEffect(() => {
        const timer = setInterval(() => setCurrentTime(new Date()), 1000);
        return () => clearInterval(timer);
    }, []);

    const fetchAdminDashboard = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<AdminDashboardResponse>(
                API_ROUTES.DASHBOARD.GET_ADMIN
            );
            setData(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : null;
            setError(message ?? "Unable to load admin dashboard summary.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchAdminDashboard();
    }, []);

    if (isLoading) {
        return (
            <Center py="xl" className="min-h-[60vh]">
                <Stack align="center" gap="md">
                    <Loader size="lg" color="indigo" />
                    <Text size="sm" c="dimmed">Loading management control panel...</Text>
                </Stack>
            </Center>
        );
    }

    const kpis = data?.kpis;
    const totalPending = (kpis?.pendingCorrectionsCount ?? 0) + (kpis?.pendingLeaveRequestsCount ?? 0);
    const attendanceTurnoutPct = kpis?.totalEmployees
        ? Math.round(((kpis.presentToday) / kpis.totalEmployees) * 100)
        : 0;

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header Banner */}
                <Paper
                    radius="lg"
                    p="xl"
                    style={{
                        background: "linear-gradient(135deg, #1e1b4b 0%, #312e81 100%)",
                        border: "1px solid rgba(255, 255, 255, 0.1)",
                        boxShadow: "0 10px 25px -5px rgba(0, 0, 0, 0.3)"
                    }}
                >
                    <Group justify="space-between" align="center" wrap="wrap">
                        <Group gap="lg">
                            <Avatar color="indigo" size={64} radius="xl" style={{ backgroundColor: "rgba(255,255,255,0.15)", color: "#ffffff" }}>
                                <Text size="xl" fw={700} c="white">
                                    {user ? `${user.firstName[0]}${user.lastName[0]}` : "M"}
                                </Text>
                            </Avatar>

                            <div>
                                <Group gap="xs" mb={2}>
                                    <Title order={2} c="white" fw={700}>
                                        Manager Control Panel 👋
                                    </Title>
                                    <Badge color="indigo" variant="filled" size="sm">Management View</Badge>
                                    <Badge color="indigo" variant="light" size="sm" leftSection={<IconClock size={14} />}>
                                        {currentTime.toLocaleTimeString()}
                                    </Badge>
                                </Group>
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
                            <Button component={Link} to="/leaves/company" variant="white" color="indigo" leftSection={<IconFileCheck size={16} />}>
                                Review Leaves
                            </Button>
                            <Button component={Link} to="/attendances/corrections/company" variant="light" color="indigo" leftSection={<IconClockCheck size={16} />}>
                                Review Corrections
                            </Button>
                        </Group>
                    </Group>
                </Paper>

                {error && (
                    <Alert color="red" title="Unable to load dashboard">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchAdminDashboard}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                {/* Sleek Minimal Pending Actions Banner */}
                {totalPending > 0 && (
                    <Paper
                        p="md"
                        radius="md"
                        style={{
                            background: "#1e1b4b",
                            borderLeft: "4px solid #f59e0b",
                            borderTop: "1px solid rgba(255, 255, 255, 0.08)",
                            borderRight: "1px solid rgba(255, 255, 255, 0.08)",
                            borderBottom: "1px solid rgba(255, 255, 255, 0.08)"
                        }}
                    >
                        <Group justify="space-between" align="center" wrap="wrap" gap="md">
                            <Group gap="sm">
                                <ThemeIcon color="amber" variant="light" size="lg" radius="md">
                                    <IconAlertCircle size={20} />
                                </ThemeIcon>

                                <div>
                                    <Group gap="xs" mb={2}>
                                        <Text size="sm" fw={600} c="white">
                                            Action Required
                                        </Text>
                                        <Badge color="amber" variant="light" size="xs">
                                            {totalPending} Pending
                                        </Badge>
                                    </Group>

                                    <Text size="xs" c="gray.3">
                                        {(() => {
                                            const leaves = kpis?.pendingLeaveRequestsCount ?? 0;
                                            const corrections = kpis?.pendingCorrectionsCount ?? 0;
                                            const parts: string[] = [];

                                            if (leaves > 0) {
                                                parts.push(`${leaves} pending leave ${leaves === 1 ? "request" : "requests"}`);
                                            }
                                            if (corrections > 0) {
                                                parts.push(`${corrections} attendance ${corrections === 1 ? "correction" : "corrections"}`);
                                            }

                                            if (parts.length === 1) {
                                                return <>You have <strong>{parts[0]}</strong> awaiting your review.</>;
                                            }

                                            return <>You have <strong>{parts[0]}</strong> and <strong>{parts[1]}</strong> awaiting your review.</>;
                                        })()}
                                    </Text>
                                </div>
                            </Group>

                            <Group gap="xs">
                                {kpis?.pendingLeaveRequestsCount ? (
                                    <Button component={Link} to="/leaves/company" size="xs" color="amber" variant="light">
                                        Review Leaves ({kpis.pendingLeaveRequestsCount})
                                    </Button>
                                ) : null}

                                {kpis?.pendingCorrectionsCount ? (
                                    <Button component={Link} to="/attendances/corrections/company" size="xs" color="amber" variant="light">
                                        Review Corrections ({kpis.pendingCorrectionsCount})
                                    </Button>
                                ) : null}
                            </Group>
                        </Group>
                    </Paper>
                )}

                {/* Top Row KPI Cards */}
                <SimpleGrid cols={{ base: 1, sm: 2, md: 4 }} spacing="lg">
                    {/* Card 1: Total Employees */}
                    <Card padding="lg" radius="md" withBorder shadow="xs">
                        <Group justify="space-between" mb="xs">
                            <Text size="xs" c="dimmed" fw={700} tt="uppercase">Total Headcount</Text>
                            <ThemeIcon size="md" radius="md" color="indigo" variant="light">
                                <IconUsers size={18} />
                            </ThemeIcon>
                        </Group>
                        <Title order={2} fw={700}>{kpis?.totalEmployees ?? 0}</Title>
                        <Text size="xs" c="dimmed" mt={4}>Active company employees</Text>
                    </Card>

                    {/* Card 2: Attendance Turnout */}
                    <Card padding="lg" radius="md" withBorder shadow="xs">
                        <Group justify="space-between" mb="xs">
                            <Text size="xs" c="dimmed" fw={700} tt="uppercase">Attendance Rate</Text>
                            <ThemeIcon size="md" radius="md" color="green" variant="light">
                                <IconUserCheck size={18} />
                            </ThemeIcon>
                        </Group>
                        <Group align="flex-end" gap="xs">
                            <Title order={2} fw={700}>{attendanceTurnoutPct}%</Title>
                            <Text size="xs" c="green" fw={600} mb={4}>Today</Text>
                        </Group>
                        <Progress value={attendanceTurnoutPct} color="green" size="xs" mt="sm" radius="xl" />
                    </Card>

                    {/* Card 3: Pending Leaves */}
                    <Card padding="lg" radius="md" withBorder shadow="xs">
                        <Group justify="space-between" mb="xs">
                            <Text size="xs" c="dimmed" fw={700} tt="uppercase">Pending Leaves</Text>
                            <ThemeIcon size="md" radius="md" color="amber" variant="light">
                                <IconFileCheck size={18} />
                            </ThemeIcon>
                        </Group>
                        <Title order={2} fw={700} c="amber.5">{kpis?.pendingLeaveRequestsCount ?? 0}</Title>
                        <Text size="xs" c="dimmed" mt={4}>Requests awaiting approval</Text>
                    </Card>

                    {/* Card 4: Time Corrections */}
                    <Card padding="lg" radius="md" withBorder shadow="xs">
                        <Group justify="space-between" mb="xs">
                            <Text size="xs" c="dimmed" fw={700} tt="uppercase">Time Corrections</Text>
                            <ThemeIcon size="md" radius="md" color="orange" variant="light">
                                <IconClockCheck size={18} />
                            </ThemeIcon>
                        </Group>
                        <Title order={2} fw={700} c="orange.5">{kpis?.pendingCorrectionsCount ?? 0}</Title>
                        <Text size="xs" c="dimmed" mt={4}>Attendance edit requests</Text>
                    </Card>
                </SimpleGrid>

                {/* Result Set 2: Top Pending Leave Requests */}
                <div>
                    <Group justify="space-between" align="center" mb="md">
                        <Title order={3}>Pending Leave Applications</Title>
                        <Button component={Link} to="/leaves/company" variant="subtle" size="xs">
                            View All Leave Approvals ➔
                        </Button>
                    </Group>
                    <Card padding="md" radius="md" withBorder>
                        {data?.pendingLeaves && data.pendingLeaves.length > 0 ? (
                            <Table highlightOnHover verticalSpacing="sm">
                                <Table.Thead>
                                    <Table.Tr>
                                        <Table.Th>Employee</Table.Th>
                                        <Table.Th>Leave Type</Table.Th>
                                        <Table.Th>Date Range</Table.Th>
                                        <Table.Th>Duration</Table.Th>
                                        <Table.Th>Reason</Table.Th>
                                    </Table.Tr>
                                </Table.Thead>
                                <Table.Tbody>
                                    {data.pendingLeaves.map((item) => (
                                        <Table.Tr key={item.id}>
                                            <Table.Td>
                                                <Text size="sm" fw={600}>{item.employeeName}</Text>
                                                <Text size="xs" c="dimmed">{item.employeeNumber}</Text>
                                            </Table.Td>
                                            <Table.Td>
                                                <Badge color="blue" variant="light">{item.leaveTypeName}</Badge>
                                            </Table.Td>
                                            <Table.Td>{item.startDate} ➔ {item.endDate}</Table.Td>
                                            <Table.Td>{item.totalDays} days</Table.Td>
                                            <Table.Td><Text size="sm" lineClamp={1}>{item.reason}</Text></Table.Td>
                                        </Table.Tr>
                                    ))}
                                </Table.Tbody>
                            </Table>
                        ) : (
                            <Text size="sm" c="dimmed" ta="center" py="md">
                                No pending leave requests.
                            </Text>
                        )}
                    </Card>
                </div>
            </Stack>
        </main>
    );
}