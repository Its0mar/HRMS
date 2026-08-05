import { Alert, Badge, Button, Group, Stack, Text, ThemeIcon, Title } from "@mantine/core";
import { DataTable, type DataTableColumn } from "../../../Common/DataTable/DataTable";
import type { RolesListItem } from "../types/RolesListItem";
import { useEffect, useState } from "react";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { IconRefresh, IconSettings, IconSettingsPlus } from "@tabler/icons-react";


export function RolesList() {

    const [roles, setRoles] = useState<RolesListItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchRoles = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<RolesListItem[]>(API_ROUTES.ROLES.GET_ALL);
            setRoles(response.data);
        }
        catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "We could not load the roles.");                
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void fetchRoles();
    }, []);


    const columns: DataTableColumn<RolesListItem>[] = [
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
            key: "name",
            header: "Name",
            render: (role) => <Text size="sm">{role.name}</Text>
        },

        {
            "key": "actions",
            "header": "Actions",
            "render": () => <Button>Edit</Button>
        }
    ]


    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indogo" variant="light">
                                <IconSettings size={22} />
                            </ThemeIcon>

                            <Title order={1}>Roles</Title>

                        </Group>
                        
                        <Text c="gray.4">
                            View and manage your organization roles.
                        </Text>
                    </div>

                        <Group>
                            <Badge size="lg" variant="light" color="indigo">
                                {roles.length} total
                            </Badge>

                            <Button leftSection={<IconSettingsPlus size={16} />}>
                                New Role
                            </Button>
                        </Group>
                </Group>

                {error && (
                    <Alert
                        color="red"
                        title="Unable to load roles">

                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button
                                size="xs"
                                variant="light"
                                color="red"
                                leftSection={<IconRefresh size={15} />}
                                onClick={fetchRoles}
                            >
                                Retry
                            </Button>
                        </Group>        
                    </Alert>
                )}

                <DataTable
                    data={roles}
                    columns={columns}
                    getRowKey={(role) => role.id}
                    isLoading={isLoading}
                    minWidth={1000}
                    emptyTitle="No roles yet"
                    emptyDescription="roles will appear here once created."
                />

            </Stack>
        </main>
    );

}