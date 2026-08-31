import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { CompanyLeaveRequestItem } from "../types/CompanyLeaveRequestItem";
import { Badge, Button, Group, Text, ThemeIcon } from "@mantine/core";
import { IconUser, IconCheck, IconX } from "@tabler/icons-react";


const renderStatusBadge = (status: number) => {
    let label = "Pending";
    let color = "yellow";
    if (status === 2){
        label = "Approved";
        color = "green";
    } else if (status === 3) {
        label = "Rejected";
        color = "red";
    } else if (status === 4) {
        label = "Cancelled";
        color = "gray";
    }
    return <Badge color={color}>{label}</Badge>;
};

export const getCompanyLeaveRequestsColumns = (
    onApprove: (item: CompanyLeaveRequestItem) => void,
    onReject: (item: CompanyLeaveRequestItem) => void,
    isSubmitting: boolean = false
): DataTableColumn<CompanyLeaveRequestItem>[] => [
    {
        key: "number",
        header: "No.",
        width: 60,
        render: (_, index) => <Text size="sm" c="dimmed">{index + 1}</Text>
    },
    {
        key: "employee",
        header: "Employee",
        render: (item) => (
            <Group gap="sm" wrap="nowrap">
                <ThemeIcon size="md" radius="xl" variant="light" color="indigo">
                    <IconUser size={16} />
                </ThemeIcon>
                <div>
                    <Text size="sm" fw={600}>{item.employeeName}</Text>
                    <Badge size="xs" variant="light" color="gray">{item.employeeCode}</Badge>
                </div>
            </Group>
        )
    },
    {
        key: "leaveType",
        header: "Leave Type",
        render: (item) => <Badge variant="light" color="blue">{item.leaveTypeName}</Badge>
    },
    {
        key: "dates",
        header: "Date Range",
        render: (item) => (
            <Text size="sm" fw={500}>
                {item.startDate} ➔ {item.endDate}
            </Text>
        )
    },
    {
        key: "totalDays",
        header: "Duration",
        render: (item) => <Text size="sm">{item.totalDays} working days</Text>
    },
    {
        key: "reason",
        header: "Reason",
        render: (item) => <Text size="sm" lineClamp={2}>{item.reason}</Text>
    },
    {
        key: "status",
        header: "Status",
            render: (item) => renderStatusBadge(item.status)

        // render: (item) => (
        //     <Badge color={item.status === "Approved" ? "green" : item.status === "Rejected" ? "red" : "yellow"}>
        //         {item.status}
        //     </Badge>
        // )
    },
    {
        key: "actions",
        header: "Actions",
        render: (item) => {
            if (item.status !== 1) {
                return (
                    <Text size="xs" c="dimmed">
                        {item.reviewedByName ? `By ${item.reviewedByName}` : "Reviewed"}
                    </Text>
                );
            }

            return (
                <Group gap="xs" wrap="nowrap">
                    <Button
                        size="xs"
                        color="green"
                        variant="light"
                        leftSection={<IconCheck size={14} />}
                        disabled={isSubmitting}
                        onClick={() => onApprove(item)}
                    >
                        Approve
                    </Button>

                    <Button
                        size="xs"
                        color="red"
                        variant="light"
                        leftSection={<IconX size={14} />}
                        disabled={isSubmitting}
                        onClick={() => onReject(item)}
                    >
                        Reject
                    </Button>
                </Group>
            );
        }
    }
];

