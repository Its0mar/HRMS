import { useEffect, useState } from "react";
import type { AttendanceListItem } from "../types/AttendanceListItem";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Badge, Button, Card, Group, Stack, ThemeIcon, Text, Notification } from "@mantine/core";
import { IconCheck, IconClock, IconLogin, IconLogout, IconX } from "@tabler/icons-react";

interface ClockWidgetProps {
    todayRecord: AttendanceListItem | null;
    onPunchSuccess: () => void;
}

export function ClockWidget({ todayRecord, onPunchSuccess }: ClockWidgetProps) {

    const [now, setNow] = useState<Date>(new Date());
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    useEffect(() => {
        const timer = setInterval(() => setNow(new Date()), 1000);
        return () => clearInterval(timer);
    }, []);
    // Determine current status
    const isClockedIn = todayRecord !== null && todayRecord !== undefined && todayRecord.clockOut === null;
    const isClockedOut = todayRecord !== null && todayRecord !== undefined && todayRecord.clockOut !== null;

    const handleClockIn = async () => {
        setIsLoading(true);
        setError(null);

        try {
            await apiClient.post(API_ROUTES.ATTENDANCES.CLOCK_IN);
            await onPunchSuccess();
        }
        catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "A problem occurred while clocking in.");
        }
        finally {
            setIsLoading(false);
        }
    }

    const handleClockOut = async () => {
        setIsLoading(true);
        setError(null);

        try {
            await apiClient.post(API_ROUTES.ATTENDANCES.CLOCK_OUT);
            await onPunchSuccess();
        }
        catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "A problem occured while clocking out.");
        }
        finally {
            setIsLoading(false);
        }
    }

    const formattedTime = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' });
    const formattedDate = now.toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric' });
    
    return (
        <Card radius="lg" shadow="md" padding="xl" withBorder>
            <Stack gap="md">
                {/* Header: Clock + Status Badge */}
                <Group justify="space-between" align="flex-start">
                    <div>
                        <Group gap="xs" mb={4}>
                            <ThemeIcon size="md" radius="md" color="indigo" variant="light">
                                <IconClock size={18} />
                            </ThemeIcon>
                            <Text size="xs" fw={700} c="dimmed" tt="uppercase">
                                Current Time
                            </Text>
                        </Group>
                        <Text size="xl" fw={700} style={{ fontSize: "2rem", lineHeight: 1.2 }}>
                            {formattedTime}
                        </Text>
                        <Text size="sm" c="dimmed" mt={2}>
                            {formattedDate}
                        </Text>
                    </div>
                    {/* Status Badge */}
                    <div>
                        {isClockedOut ? (
                            <Badge size="lg" color="gray" variant="light" leftSection={<IconCheck size={14} />}>
                                Completed for today
                            </Badge>
                        ) : isClockedIn ? (
                            <Badge size="lg" color="teal" variant="light" leftSection={<IconClock size={14} />}>
                                Clocked In ({formatLocalTime(todayRecord.clockIn)})
                            </Badge>
                        ) : (
                            <Badge size="lg" color="yellow" variant="light">
                                Not Clocked In
                            </Badge>
                        )}
                    </div>
                </Group>
                {/* Error Banner */}
                {error && (
                    <Notification color="red" icon={<IconX size={18} />} onClose={() => setError(null)}>
                        {error}
                    </Notification>
                )}
                {/* Action Buttons */}
                <Group grow mt="xs">
                    <Button
                        size="md"
                        color="teal"
                        variant="filled"
                        leftSection={<IconLogin size={20} />}
                        loading={isLoading}
                        disabled={isClockedIn || isClockedOut || isLoading}
                        onClick={handleClockIn}
                    >
                        Clock In
                    </Button>
                    <Button
                        size="md"
                        color="orange"
                        variant="filled"
                        leftSection={<IconLogout size={20} />}
                        loading={isLoading}
                        disabled={!isClockedIn || isLoading}
                        onClick={handleClockOut}
                    >
                        Clock Out
                    </Button>
                </Group>
            </Stack>
        </Card>
    );
}

const formatLocalTime = (isoString: string | null) => {
    if (!isoString) return "—";
    const date = new Date(isoString);
    return isNaN(date.getTime())
        ? isoString
        : date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};