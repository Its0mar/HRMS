import type { MyLeaveBalanceItem } from "../types/MyLeaveBalanceItem";
import { Badge, Card, Group, SimpleGrid, Stack, Text, ThemeIcon } from "@mantine/core";
import { IconCalendar, IconClock, IconCheck, IconSun } from "@tabler/icons-react";

interface LeaveBalanceCardsProps {
    balances: MyLeaveBalanceItem[];
}

export function LeaveBalanceCards({ balances }: LeaveBalanceCardsProps) {
    if (balances.length === 0) {
        return (
            <Card padding="lg" radius="md" withBorder>
                <Text c="dimmed" ta="center">No leave balance categories available.</Text>
            </Card>
        );
    }

    return (
        <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }} spacing="lg">
            {balances.map((item) => (
                <Card key={item.leaveTypeId} padding="lg" radius="md" withBorder shadow="xs">
                    <Group justify="space-between" align="flex-start" mb="sm">
                        <Group gap="xs">
                            <ThemeIcon size="md" radius="md" variant="light" color={item.isPaid ? "indigo" : "gray"}>
                                <IconSun size={18} />
                            </ThemeIcon>
                            <div>
                                <Text fw={600} size="md">{item.leaveTypeName}</Text>
                                <Badge size="xs" variant="light" color="indigo">{item.leaveTypeCode}</Badge>
                            </div>
                        </Group>

                        <Badge color={item.remainingDays > 0 ? "green" : "red"} variant="light" size="lg">
                            {item.remainingDays} Days Left
                        </Badge>
                    </Group>

                    <Stack gap="xs" mt="md">
                        <Group justify="space-between">
                            <Group gap={4}>
                                <IconCalendar size={14} className="text-gray-400" />
                                <Text size="xs" c="dimmed">Entitled Total:</Text>
                            </Group>
                            <Text size="xs" fw={600}>{item.totalEntitledDays} days</Text>
                        </Group>

                        <Group justify="space-between">
                            <Group gap={4}>
                                <IconCheck size={14} className="text-emerald-500" />
                                <Text size="xs" c="dimmed">Used:</Text>
                            </Group>
                            <Text size="xs" fw={600} c="green">{item.usedDays} days</Text>
                        </Group>

                        <Group justify="space-between">
                            <Group gap={4}>
                                <IconClock size={14} className="text-amber-500" />
                                <Text size="xs" c="dimmed">Pending Approval:</Text>
                            </Group>
                            <Text size="xs" fw={600} c="amber">{item.pendingDays} days</Text>
                        </Group>
                    </Stack>
                </Card>
            ))}
        </SimpleGrid>
    );
}