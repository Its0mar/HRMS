import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { CompanyAttendanceItem } from "../types/CompanyAttendanceItem";
import { Badge, Group, Text, ThemeIcon } from "@mantine/core";
import { IconUser, IconBuildingCommunity } from "@tabler/icons-react";

const formatLocalTime = (isoString: string | null) => {
    if (!isoString) return "—";
    const date = new Date(isoString);
    return isNaN(date.getTime())
        ? isoString
        : date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};

export const CompanyAttendanceColumns: DataTableColumn<CompanyAttendanceItem>[] = [
    {
        key: "number",
        header: "No.",
        width: 60,
        render: (_, index) => (
            <Text size="sm" c="dimmed">
                {index + 1}
            </Text>
        )
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
        key: "department",
        header: "Department",
        render: (item) => (
            item.departmentName ? (
                <Group gap="xs" wrap="nowrap">
                    <ThemeIcon size="xs" radius="xl" variant="light" color="teal">
                        <IconBuildingCommunity size={12} />
                    </ThemeIcon>
                    <Text size="sm">{item.departmentName}</Text>
                </Group>
            ) : (
                <Text size="sm" c="dimmed">Unassigned</Text>
            )
        )
    },
    {
        key: "date",
        header: "Date",
        render: (item) => <Text size="sm" fw={500}>{item.date}</Text>
    },
    {
        key: "clockIn",
        header: "Clock In",
        render: (item) => <Text size="sm" c="teal.4" fw={500}>{formatLocalTime(item.clockIn)}</Text>
    },
    {
        key: "clockOut",
        header: "Clock Out",
        render: (item) => (
            <Text size="sm" c={item.clockOut ? "orange.4" : "dimmed"}>
                {formatLocalTime(item.clockOut)}
            </Text>
        )
    },
    {
        key: "status",
        header: "Status",
        render: (item) => (
            <Badge
                variant="light"
                color={
                    item.status === "Present" ? "green"
                    : item.status === "Late" ? "yellow"
                    : item.status === "HalfDay" ? "blue"
                    : item.status === "Absent" ? "red"
                    : "gray"
                }
            >
                {item.status}
            </Badge>
        )
    },
    {
        key: "totalMinutes",
        header: "Total Work",
        render: (item) => (
            <Text size="sm">
                {item.totalMinutes !== null ? `${Math.floor(item.totalMinutes / 60)}h ${item.totalMinutes % 60}m` : "—"}
            </Text>
        )
    },
    {
        key: "lateMinutes",
        header: "Late Mins",
        render: (item) => (
            <Text size="sm" c={item.lateMinutes > 0 ? "yellow.5" : "dimmed"}>
                {item.lateMinutes > 0 ? `${item.lateMinutes} mins` : "On Time"}
            </Text>
        )
    }
];