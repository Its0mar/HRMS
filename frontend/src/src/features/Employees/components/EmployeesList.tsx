import { useEffect, useState } from "react";
import type { EmployeeListItem } from "../types/EmployeeListItem";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { apiClient } from "../../../lib/apiClient";
import axios from "axios";
import { Badge, Button, Group, Stack, ThemeIcon, Title, Text, Alert, Card, Table, Skeleton, Center } from "@mantine/core";
import { IconRefresh, IconUser, IconUserPlus } from "@tabler/icons-react";

export function EmployeesList() {

    const [employees, setEmployees] = useState<EmployeeListItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchEmployees = async () => {
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

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="light">
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

                            <Button leftSection={<IconUserPlus size={16} />}>
                                New Employee
                            </Button>
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

                <Card 
                    padding={0}
                    radius="lg"
                    shadow="lg"
                    withBorder
                >
                    <Table.ScrollContainer minWidth={1000}>
                        <Table 
                            verticalSpacing="md"
                            horizontalSpacing="lg"
                            highlightOnHover
                        >

                            <Table.Thead bg="gray.1">
                                <Table.Tr>
                                    <Table.Th w={70}>No.</Table.Th>
                                    <Table.Th>Employee</Table.Th>
                                    <Table.Th>Department</Table.Th>
                                    <Table.Th>Position</Table.Th>
                                    <Table.Th>Type</Table.Th>
                                    <Table.Th>Status</Table.Th>
                                    <Table.Th>Actions</Table.Th>
                                </Table.Tr>
                            </Table.Thead>

                            <Table.Tbody>
                                {isLoading &&
                                    Array.from({ length: 5 }).map((_, rowIndex) => (
                                        <Table.Tr key={rowIndex}>
                                            {Array.from({ length: 7 }).map(
                                                (_, cellIndex) => (
                                                    <Table.Td key={cellIndex}>
                                                        <Skeleton height={18} />
                                                    </Table.Td>
                                                )
                                            )}
                                        </Table.Tr>
                                    ))}

                                {!isLoading && 
                                    employees.map((employee, index) => (
                                        <Table.Tr key={employee.id}>
                                            <Table.Td>
                                                <Text size="sm" c="dimmed">
                                                    {index + 1}
                                                </Text>
                                            </Table.Td>

                                            <Table.Td>
                                                <Group gap="sm" wrap="nowrap">
                                                    <ThemeIcon
                                                        size="lg"
                                                        radius="xl"
                                                        color="indigo"
                                                        variant="light"
                                                    >
                                                        <IconUser size={17} />
                                                    </ThemeIcon>

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
                                                </Group>
                                            </Table.Td>

                                            <Table.Td>
                                                <Text size="sm">
                                                    {employee.departmentName || "Unassigned"}
                                                </Text>
                                            </Table.Td>

                                            <Table.Td>
                                                <Text size="sm">
                                                    {employee.positionName || "Unassigned"}
                                                </Text>
                                            </Table.Td>

                                            <Table.Td>
                                                <Badge variant="light" color="blue">
                                                    {employee.employmentType}
                                                </Badge>
                                            </Table.Td>

                                            <Table.Td>
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
                                            </Table.Td>

                                            <Table.Td>
                                                <Button size="xs" variant="light">
                                                    View
                                                </Button>
                                            </Table.Td>

                                        </Table.Tr>
                                    ))
                                }
                            </Table.Tbody>
                        </Table>

                        {!isLoading && !error && employees.length === 0 && (
                            <Center py={60}>
                                <Stack align="center" gap="xs">
                                    <ThemeIcon
                                        size={52}
                                        radius="xl"
                                        variant="light"
                                        color="gray"
                                    >
                                        <IconUser size={26} />
                                    </ThemeIcon>

                                    <Text fw={600}>
                                        No employees yet
                                    </Text>

                                    <Text size="sm" c="dimmed">
                                        Employees will appear here once created.
                                    </Text>
                                </Stack>
                            </Center>
                        )}
                    </Table.ScrollContainer>
                        

                </Card>


            </Stack>
        </main>
    );
}