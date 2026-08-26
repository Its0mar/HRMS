import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { LeaveTypeListItem } from "../types/LeaveTypeListItem";
import { Badge, Button } from "@mantine/core";

export const getLeaveTypesListColumns = (
    onEdit: (item: LeaveTypeListItem) => void
): DataTableColumn<LeaveTypeListItem>[] => [
    {
        key: "name",
        header: "Name",
        render: (item) => item.name,
    },
    {
        key: "code",
        header: "Code",
        render: (item) => <Badge variant="light" color="indigo">{item.code}</Badge>,
    },
    {
        key: "defaultDaysPerYear",
        header: "Default Days / Year",
        render: (item) => `${item.defaultDaysPerYear} days`,
    },
    {
        key: "isPaid",
        header: "Is Paid",
        render: (item) => (
            <Badge color={item.isPaid ? "green" : "gray"} variant="light">
                {item.isPaid ? "Paid" : "Unpaid"}
            </Badge>
        ),
    },
    {
        key: "requiresApproval",
        header: "Requires Approval",
        render: (item) => (
            <Badge color={item.requiresApproval ? "blue" : "gray"} variant="light">
                {item.requiresApproval ? "Yes" : "No"}
            </Badge>
        ),
    },
    {
        key: "actions",
        header: "Actions",
        render: (item) => (
            <Button
                size="xs"
                variant="light"
                onClick={() => onEdit(item)}
            >
                Edit
            </Button>
        ),
    },
];