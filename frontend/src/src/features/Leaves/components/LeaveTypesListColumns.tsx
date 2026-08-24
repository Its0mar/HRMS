import type { DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { LeaveTupeListItem } from "../types/LeaveTypeListItem";

export const LeaveTypesListColumns : DataTableColumn<LeaveTupeListItem>[] = [
    {
        key: "name",
        header: "Name",
        render: (item) => item.name,
    },
    {
        key: "code",
        header: "Code",
        render: (item) => item.code,
    },
    {
        key: "defaultDaysPerYear",
        header: "Default Days Per Year",
        render: (item) => item.defaultDaysPerYear,
    },
    {
        key: "isPaid",
        header: "Is Paid",
        render: (item) => item.isPaid ? "Paid" : "Not Paid",
    },
    {
        key: "requiresApproval",
        header: "Requires Approval",
        render: (item) => item.isPaid ? "Yes" : "No",
    },
]