import { Button, Group, Text } from "@mantine/core";
import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { AttendanceCorrectionListItem } from "../types/AttendanceCorrectionListItem";
import { IconCheck, IconX } from "@tabler/icons-react";

export const GetCompanyAttendanceCorrectionsColumns = (
    onApproveOrReject: (id: number, action: "approve" | "reject") => void,
    isSubmitting: boolean = false
): DataTableColumn<AttendanceCorrectionListItem>[] => [
        {
            key: "number",
            header: "No.",
            width: 60,
            render: (_, index) => index + 1,
        },
        {
            key: "employeeNumber",
            header: "Employee Number",
            render: (item) => item.employeeNumber,
        },
        {
            key: "employeeName",
            header: "Employee Name",
            render: (item) => item.employeeName,
        },
        {
            key: "requestedClockIn",
            header: "Requested Clock In",
            render: (item) => formatLocalTime(item.requestedClockIn)
        },
        {
            key: "requestedClockOut",
            header: "Requested Clock Out",
            render: (item) => formatLocalTime(item.requestedClockOut)
        },
        {
            key: "reason",
            header: "Reason",
            render: (item) => item.reason
        },
        {
            key: "actions",
            header: "Actions",
            render: (item) => {
                // Only show Action buttons if the status is "Pending" (or 1)
                if (item.status !== 1) {
                    return <Text size="xs" c="dimmed">Reviewed</Text>;
                }
                return (
                    <Group gap="xs" wrap="nowrap">
                        <Button
                            size="xs"
                            color="green"
                            variant="light"
                            leftSection={<IconCheck size={14} />}
                            disabled={isSubmitting}
                            onClick={() => onApproveOrReject(item.id, "approve")}
                        >
                            Approve
                        </Button>
                        <Button
                            size="xs"
                            color="red"
                            variant="light"
                            leftSection={<IconX size={14} />}
                            disabled={isSubmitting}
                            onClick={() => onApproveOrReject(item.id, "reject")}
                        >
                            Reject
                        </Button>
                    </Group>
                );
            }
        }

    ];



    const formatLocalTime = (isoString: string | null) => {
    if (!isoString) return "—";
    const date = new Date(isoString);
    return isNaN(date.getTime())
        ? isoString
        : date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};