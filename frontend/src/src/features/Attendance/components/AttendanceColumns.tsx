import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { AttendanceListItem } from "../types/AttendanceListItem";
import { Badge, Text } from "@mantine/core";

export const AttendanceColumns: DataTableColumn<AttendanceListItem>[] = [
    {
        key: "number",
        header: "No.",
        width: 70,
        render: (_, index) => (
            <Text size="sm" c="dimmed">
                {index + 1}
            </Text>
        )
    },
    {
        key: "date",
        header: "Date",
        render: (attendance) => <Text size="sm" fw={500}>{attendance.date.toString()}</Text>
    },
    {
        key: "clockIn",
        header: "Clock In",
        render: (attendance) => <Text size="sm" c="teal.9">{formatLocalTime(attendance.clockIn)}</Text>
    },
    {
        key: "clockOut",
        header: "Clock Out",
        render: (attendance) => (
            <Text size="sm" c={attendance.clockOut ? "orange.9" : "dimmed"}>
                {formatLocalTime(attendance.clockOut) ?? "—"}
            </Text>
        )
    },
    {
        key: "status",
        header: "Status",
        render: (attendance) => (
            <Badge
                variant="light"
                color={
                    attendance.status === "Present" ? "green"
                    : attendance.status === "Late" ? "yellow"
                    : attendance.status === "HalfDay" ? "blue"
                    : attendance.status === "Absent" ? "red"
                    : "gray"
                }
            >
                {attendance.status}
            </Badge>
        )
    },
    {
        key: "totalMinutes",
        header: "Total Work",
        render: (attendance) => (
            <Text size="sm">
                {attendance.totalMinutes ? `${Math.floor(attendance.totalMinutes / 60)}h ${attendance.totalMinutes % 60}m` : "—"}
            </Text>
        )
    },
    {
        key: "lateMinutes",
        header: "Late Mins",
        render: (attendance) => (
            <Text size="sm" c={attendance.lateMinutes > 0 ? "yellow.9" : "dimmed"}>
                {attendance.lateMinutes > 0 ? `${attendance.lateMinutes} mins` : "On Time"}
            </Text>
        )
    },
    {
        key: "overtimeMinutes",
        header: "Overtime",
        render: (attendance) => (
            <Text size="sm" c={attendance.overtimeMinutes > 0 ? "teal.9" : "dimmed"}>
                {attendance.overtimeMinutes > 0 ? `${attendance.overtimeMinutes} mins` : "0"}
            </Text>
        )
    }
];

const formatLocalTime = (isoString: string | null) => {
    if (!isoString) return "—";
    const date = new Date(isoString);
    return isNaN(date.getTime())
        ? isoString
        : date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};