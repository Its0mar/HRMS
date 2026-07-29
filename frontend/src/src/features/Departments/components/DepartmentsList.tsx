import { useEffect, useState } from "react";
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
import { IconBuildingCommunity, IconRefresh } from "@tabler/icons-react";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";

interface Department {
    id: number;
    name: string;
}

export function DepartmentsList() {
    const [departments, setDepartments] = useState<Department[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

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

                    <Badge size="lg" variant="light" color="indigo">
                        {departments.length} total
                    </Badge>
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

                <Card padding={0} radius="lg" shadow="lg" withBorder className="!bg-gray-500">
                    <Table.ScrollContainer minWidth={500}>
                        <Table
                            verticalSpacing="md"
                            horizontalSpacing="xl"
                            highlightOnHover
                        >
                            <Table.Thead bg="gray.1">
                                <Table.Tr>
                                    <Table.Th w={100}>ID</Table.Th>
                                    <Table.Th>Department name</Table.Th>
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
                                        </Table.Tr>
                                    ))}

                                {!isLoading &&
                                    departments.map((department) => (
                                        <Table.Tr key={department.id}>
                                            <Table.Td>
                                                <Text size="sm" c="dimmed">
                                                    #{department.id}
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
                                                    <Text fw={500}>{department.name}</Text>
                                                </Group>
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
                </Card>
            </Stack>
        </main>
    );
}
