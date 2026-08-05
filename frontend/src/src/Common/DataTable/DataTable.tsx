import type { Key, ReactNode } from "react";
import {
    Card,
    Center,
    Skeleton,
    Stack,
    Table,
    Text
} from "@mantine/core";

export interface DataTableColumn<T> {
    key: string;
    header: ReactNode;
    width?: number | string;
    render: (item: T, index: number) => ReactNode;
}

interface DataTableProps<T> {
    data: T[];
    columns: DataTableColumn<T>[];
    getRowKey: (item: T) => Key;
    isLoading?: boolean;
    loadingRows?: number;
    minWidth?: number;
    emptyTitle?: string;
    emptyDescription?: string;
}

export function DataTable<T>({
    data,
    columns,
    getRowKey,
    isLoading = false,
    loadingRows = 5,
    minWidth = 800,
    emptyTitle = "No records found",
    emptyDescription
}: DataTableProps<T>) {
    return (
        <Card padding={0} radius="lg" shadow="lg" withBorder>
            <Table.ScrollContainer minWidth={minWidth}>
                <Table
                    verticalSpacing="md"
                    horizontalSpacing="lg"
                    highlightOnHover
                >
                    <Table.Thead bg="gray.1">
                        <Table.Tr>
                            {columns.map((column) => (
                                <Table.Th
                                    key={column.key}
                                    w={column.width}
                                >
                                    {column.header}
                                </Table.Th>
                            ))}
                        </Table.Tr>
                    </Table.Thead>

                    <Table.Tbody>
                        {isLoading &&
                            Array.from({ length: loadingRows }).map(
                                (_, rowIndex) => (
                                    <Table.Tr key={rowIndex}>
                                        {columns.map((column) => (
                                            <Table.Td key={column.key}>
                                                <Skeleton height={18} />
                                            </Table.Td>
                                        ))}
                                    </Table.Tr>
                                )
                            )}

                        {!isLoading &&
                            data.map((item, index) => (
                                <Table.Tr key={getRowKey(item)}>
                                    {columns.map((column) => (
                                        <Table.Td key={column.key}>
                                            {column.render(item, index)}
                                        </Table.Td>
                                    ))}
                                </Table.Tr>
                            ))}
                    </Table.Tbody>
                </Table>

                {!isLoading && data.length === 0 && (
                    <Center py={60}>
                        <Stack align="center" gap="xs">
                            <Text fw={600}>{emptyTitle}</Text>

                            {emptyDescription && (
                                <Text size="sm" c="dimmed">
                                    {emptyDescription}
                                </Text>
                            )}
                        </Stack>
                    </Center>
                )}
            </Table.ScrollContainer>
        </Card>
    );
}