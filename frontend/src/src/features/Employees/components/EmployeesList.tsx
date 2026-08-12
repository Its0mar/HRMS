import { useEffect, useState } from "react";
import type { EmployeeListItem } from "../types/EmployeeListItem";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { apiClient } from "../../../lib/apiClient";
import axios from "axios";
import { Badge, Button, Group, Stack, ThemeIcon, Title, Text, Alert } from "@mantine/core";
import { IconRefresh, IconUser, IconUserPlus } from "@tabler/icons-react";
import { DataTable, type DataTableColumn } from "../../../Common/DataTable/DataTable";
import { useDisclosure } from "@mantine/hooks";
import { CreateAccessModal } from "./CreateAccessModal";
import { UpdateAccessModal } from "./UpdateAccessModal";
import { PERMISSIONS } from "../../Auth/constants/permissions";
import { usePermission } from "../../Auth/hooks/usePermission";

export function EmployeesList() {

    const [employees, setEmployees] = useState<EmployeeListItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [createAccessOpened, createAccessModal] = useDisclosure(false);
    const [updateAccessOpened, updateAccessModal] = useDisclosure(false);

    const [selectedEmployee, setSelectedEmployee] =
        useState<{
            id: number;
            name: string;
        } | null>(null);

    const canCreateEmployee = usePermission(PERMISSIONS.EMPLOYEES.CREATE);    


    const fetchEmployees = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<EmployeeListItem[]>(API_ROUTES.EMPLOYEES.GET_ALL);
            setEmployees(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "We could not load the employees.");
        } finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void fetchEmployees();
    }, []);


    const handleCreateAccess = (
        employeeId: number,
        employeeName: string,
    ) => {
        setSelectedEmployee({
            id: employeeId,
            name: employeeName,
        });

        createAccessModal.open();
    };

    const handleUpdateAccess = (
        employeeId: number,
        employeeName: string,
    ) => {
        setSelectedEmployee({
            id: employeeId,
            name: employeeName,
        });

        updateAccessModal.open();
    };

    const handleCreateAccessClose = () => {
        createAccessModal.close();
        setSelectedEmployee(null);
    };

    const handleUpdateAccessClose = () => {
        updateAccessModal.close();
        setSelectedEmployee(null);
    };



    const columns: DataTableColumn<EmployeeListItem>[] = [
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
            key: "employee",
            header: "Employee",
            render: (employee) => (
                <div>
                    <Text fw={600}>
                        {employee.fullName}
                    </Text>

                    <Text size="xs" c="dimmed">
                        {employee.employeeNumber}
                    </Text>

                    <Text size="xs" c="dimmed">
                        {employee.workEmail}
                    </Text>
                </div>
            )
        },
        {
            key: "department",
            header: "Department",
            render: (employee) => (
                <Text size="sm">
                    {employee.departmentName || "Unassigned"}
                </Text>
            )
        },
        {
            key: "position",
            header: "Position",
            render: (employee) => (
                <Text size="sm">
                    {employee.positionName || "Unassigned"}
                </Text>
            )
        },
        {
            key: "type",
            header: "Type",
            render: (employee) => (
                <Badge variant="light" color="blue">
                    {employee.employmentType}
                </Badge>
            )
        },
        {
            key: "status",
            header: "Status",
            render: (employee) => (
                <Badge
                    variant="light"
                    color={
                        employee.employmentStatus === "Active"
                            ? "green"
                            : "gray"
                    }
                >
                    {employee.employmentStatus}
                </Badge>
            )
        },
        {
            key: "actions",
            header: "Actions",
            render: (employee) => (
                <Group>
                    <Button
                        size="xs"
                        variant="light"
                        onClick={() => console.log(employee.id)}
                    >
                        View
                    </Button>

                    {!employee.hasUserAccess && (
                        <Button
                            size="xs"
                            variant="light"
                            onClick={() =>
                                handleCreateAccess(
                                    employee.id,
                                    employee.fullName,
                                )
                            }
                        >
                            Create Access
                        </Button>
                    )}

                    {employee.hasUserAccess && (
                        <Button
                            size="xs"
                            variant="light"
                            onClick={() =>
                                handleUpdateAccess(
                                    employee.id,
                                    employee.fullName,
                                )
                            }
                        >
                            Update Access
                        </Button>
                    )}



                </Group>
            )
        }
    ];

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indogo" variant="light">
                                <IconUser size={22} />
                            </ThemeIcon>

                            <Title order={1}>Employees</Title>

                        </Group>

                        <Text c="gray.4">
                            View and manage your organization employees.
                        </Text>
                    </div>

                    <Group>
                        <Badge size="lg" variant="light" color="indigo">
                            {employees.length} total
                        </Badge>

                        {canCreateEmployee && <Button leftSection={<IconUserPlus size={16} />}>
                            New Employee
                        </Button>}
                    </Group>
                </Group>

                {error && (
                    <Alert
                        color="red"
                        title="Unable to load employees">

                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button
                                size="xs"
                                variant="light"
                                color="red"
                                leftSection={<IconRefresh size={15} />}
                                onClick={fetchEmployees}
                            >
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                <DataTable
                    data={employees}
                    columns={columns}
                    getRowKey={(employee) => employee.id}
                    isLoading={isLoading}
                    minWidth={1000}
                    emptyTitle="No employees yet"
                    emptyDescription="Employees will appear here once created."
                />

                <CreateAccessModal
                    opened={createAccessOpened}
                    employeeId={selectedEmployee?.id ?? null}
                    employeeName={selectedEmployee?.name ?? null}
                    onClose={handleCreateAccessClose}
                    onCreated={() => {
                        void fetchEmployees();
                    }}
                />

                <UpdateAccessModal
                    opened={updateAccessOpened}
                    employeeId={selectedEmployee?.id ?? null}
                    employeeName={selectedEmployee?.name ?? null}
                    onClose={handleUpdateAccessClose}
                    onUpdated={() => {
                        void fetchEmployees();
                    }}
                />

            </Stack>
        </main>
    );
}