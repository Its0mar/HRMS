import { Modal, Stack, Group, Text, Badge, Divider, Button } from "@mantine/core";
import type { CompanyLeaveRequestItem } from "../types/CompanyLeaveRequestItem";

interface Props {
    opened: boolean;
    onClose: () => void;
    request: CompanyLeaveRequestItem | null;
}

export function ViewLeaveDetailsModal({ opened, onClose, request }: Props) {
    if (!request) return null;

    return (
        <Modal opened={opened} onClose={onClose} title="Leave Application Details" centered radius="md">
            <Stack gap="md">
                <Group justify="space-between">
                    <div>
                        <Text size="xs" c="dimmed">Employee</Text>
                        <Text fw={600}>{request.employeeName}</Text>
                        <Text size="xs" c="dimmed">{request.employeeCode}</Text>
                    </div>
                    <Badge size="lg" color="indigo" variant="light">
                        {request.leaveTypeName}
                    </Badge>
                </Group>

                <Divider />

                <Group justify="space-between">
                    <div>
                        <Text size="xs" c="dimmed">Start Date</Text>
                        <Text size="sm" fw={600}>{request.startDate}</Text>
                    </div>
                    <div>
                        <Text size="xs" c="dimmed">End Date</Text>
                        <Text size="sm" fw={600}>{request.endDate}</Text>
                    </div>
                    <div>
                        <Text size="xs" c="dimmed">Duration</Text>
                        <Text size="sm" fw={600}>{request.totalDays} Days</Text>
                    </div>
                </Group>

                <Divider />

                <div>
                    <Text size="xs" c="dimmed" mb={4}>Reason for Leave</Text>
                    <Text size="sm" style={{ whiteSpace: "pre-wrap" }}>
                        {request.reason || "No reason provided."}
                    </Text>
                </div>

                <Group justify="flex-end" mt="md">
                    <Button variant="light" color="indigo" onClick={onClose}>
                        Close
                    </Button>
                </Group>
            </Stack>
        </Modal>
    );
}
