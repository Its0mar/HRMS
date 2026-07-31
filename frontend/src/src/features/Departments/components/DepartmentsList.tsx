import { useEffect, useState } from "react";
import { IconPlus } from "@tabler/icons-react";
import { CreateDepartmentModal } from "./CreateDepartmentModal";
import axios from "axios";
import {
    Alert,
    Badge,
    Button,
    Card,
    Center,
    Group,
    Skeleton,
    Stack,
    Table,
    Text,
    ThemeIcon,
    Title
} from "@mantine/core";
import {
    IconBuildingCommunity,
    IconRefresh,
    IconUser
} from "@tabler/icons-react";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { useDisclosure } from "@mantine/hooks";
import { UpdateDepartmentModal } from "./UpdateDepartmentModal";
import type { Department } from "../types/Department";

export function DepartmentsList() {
    const [departments, setDepartments] = useState<Department[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedDepartment, setSelectedDepartment] = useState<Department | null>(null);
    const [updateOpened, updateModal] = useDisclosure(false);
    const [createOpened, createModal] = useDisclosure(false);
    

    const handleEdit = (department: Department) => {
    setSelectedDepartment(department);
    updateModal.open();
    };

    const fetchDepartments = async () => {
        setIsLoading(true);
        setError(null);

        try {
            const response = await apiClient.get<Department[]>(
                API_ROUTES.DEPARTMENTS.GET_ALL
            );

            setDepartments(response.data);
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "We could not load the departments.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchDepartments();
    }, []);

    return (
        <main className="mx-auto w-full max-w-5xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" variant="light">
                                <IconBuildingCommunity size={22} />
                            </ThemeIcon>

                            <Title order={1} c="white">
                                Departments
                            </Title>
                        </Group>

                        <Text c="gray.4">
                            View and manage your organization departments.
                        </Text>
                    </div>

                    <Group>
                        <Badge size="lg" variant="light" color="indigo">
                            {departments.length} total
                        </Badge>

                        <Button
                            leftSection={<IconPlus size={16} />}
                            onClick={createModal.open}
                        >
                            New department
                        </Button>
                    </Group>
                </Group>

                {error && (
                    <Alert color="red" title="Unable to load departments">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button
                                size="xs"
                                color="red"
                                variant="light"
                                leftSection={<IconRefresh size={15} />}
                                onClick={fetchDepartments}
                            >
                                Try again
                            </Button>
                        </Group>
                    </Alert>
                )}

                <Card padding={0} radius="lg" shadow="lg" withBorder>
                    <Table.ScrollContainer minWidth={850}>
                        <Table
                            verticalSpacing="md"
                            horizontalSpacing="xl"
                            highlightOnHover
                        >
                            <Table.Thead bg="gray.1">
                                <Table.Tr>
                                    <Table.Th w={80}>No.</Table.Th>
                                    <Table.Th>Name</Table.Th>
                                    <Table.Th>Description</Table.Th>
                                    <Table.Th>Manager</Table.Th>
                                    <Table.Th>Edit</Table.Th>
                                </Table.Tr>
                            </Table.Thead>

                            <Table.Tbody>
                                {isLoading &&
                                    Array.from({ length: 4 }).map((_, index) => (
                                        <Table.Tr key={index}>
                                            <Table.Td>
                                                <Skeleton height={18} width={35} />
                                            </Table.Td>
                                            <Table.Td>
                                                <Skeleton height={18} width="45%" />
                                            </Table.Td>
                                            <Table.Td>
                                                <Skeleton height={18} width="70%" />
                                            </Table.Td>
                                            <Table.Td>
                                                <Skeleton height={18} width="55%" />
                                            </Table.Td>
                                            <Table.Td>
                                                <Skeleton height={30} width={50} />
                                            </Table.Td>
                                        </Table.Tr>
                                    ))}

                                {!isLoading &&
                                    departments.map((department, index) => (
                                        <Table.Tr key={department.id}>
                                            <Table.Td>
                                                <Text size="sm" c="dimmed">
                                                    {index + 1}
                                                </Text>
                                            </Table.Td>
                                            <Table.Td>
                                                <Group gap="sm">
                                                    <ThemeIcon
                                                        size="sm"
                                                        radius="xl"
                                                        variant="light"
                                                        color="indigo"
                                                    >
                                                        <IconBuildingCommunity size={13} />
                                                    </ThemeIcon>
                                                    <div>
                                                        <Text fw={600}>
                                                            {department.name}
                                                        </Text>
                                                        <Badge
                                                            size="xs"
                                                            variant="light"
                                                            color="indigo"
                                                        >
                                                            {department.code}
                                                        </Badge>
                                                    </div>
                                                </Group>
                                            </Table.Td>
                                            <Table.Td>
                                                <Text
                                                    size="sm"
                                                    c={department.description?.trim()
                                                        ? undefined
                                                        : "dimmed"}
                                                    lineClamp={2}
                                                >
                                                    {department.description?.trim() ||
                                                        "No description"}
                                                </Text>
                                            </Table.Td>
                                            <Table.Td>
                                                {department.managerEmployeeId &&
                                                department.managerEmployeeId > 0 &&
                                                department.managerName?.trim() ? (
                                                    <Group gap="xs" wrap="nowrap">
                                                        <ThemeIcon
                                                            size="sm"
                                                            radius="xl"
                                                            variant="light"
                                                            color="teal"
                                                        >
                                                            <IconUser size={13} />
                                                        </ThemeIcon>
                                                        <Text size="sm" fw={500}>
                                                            {department.managerName}
                                                        </Text>
                                                    </Group>
                                                ) : (
                                                    <Badge
                                                        color="gray"
                                                        variant="light"
                                                        fw={500}
                                                    >
                                                        Unassigned
                                                    </Badge>
                                                )}
                                            </Table.Td>
                                            <Table.Td>
                                                <Button
                                                    size="xs"
                                                    variant="light"
                                                    onClick={() => handleEdit(department)}
                                                >
                                                    Edit
                                                </Button>
                                            </Table.Td>
                                        </Table.Tr>
                                    ))}
                            </Table.Tbody>
                        </Table>

                        {!isLoading && !error && departments.length === 0 && (
                            <Center py={60}>
                                <Stack align="center" gap="xs">
                                    <ThemeIcon
                                        size={52}
                                        radius="xl"
                                        variant="light"
                                        color="gray"
                                    >
                                        <IconBuildingCommunity size={26} />
                                    </ThemeIcon>
                                    <Text fw={600}>No departments yet</Text>
                                    <Text size="sm" c="dimmed">
                                        Departments will appear here once created.
                                    </Text>
                                </Stack>
                            </Center>
                        )}
                    </Table.ScrollContainer>
                    <UpdateDepartmentModal
                        department={selectedDepartment}
                        opened={updateOpened}
                        onClose={() => {
                            updateModal.close();
                            setSelectedDepartment(null);
                        }}
                        onUpdated={fetchDepartments}
                    />

                    <CreateDepartmentModal
                    opened={createOpened}
                    onClose={createModal.close}
                    onCreated={fetchDepartments}
                    />
                </Card>
            </Stack>
        </main>
    );
}
