import { useState } from "react";
import {
    Alert,
    Button,
    Group,
    Modal,
    Stack,
    Textarea,
    TextInput
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { zod4Resolver } from "mantine-form-zod-resolver";
import axios from "axios";
import z from "zod";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { EmployeeSelect } from "../../Employees/components/EmployeeSelect";

const schema = z.object({
    name: z
        .string()
        .min(3, "Name must contain at least 3 characters")
        .max(100),

    code: z
        .string()
        .min(2, "Code must contain at least 2 characters")
        .max(20),

    description: z.string().max(300),

    managerId: z.string().nullable()
});

type CreateDepartmentValues = z.infer<typeof schema>;

interface CreateDepartmentModalProps {
    opened: boolean;
    onClose: () => void;
    onCreated: () => void;
}

export function CreateDepartmentModal({
    opened,
    onClose,
    onCreated
}: CreateDepartmentModalProps) {
    const [isCreating, setIsCreating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<CreateDepartmentValues>({
        validate: zod4Resolver(schema),
        initialValues: {
            name: "",
            code: "",
            description: "",
            managerId: null
        }
    });

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (
        values: CreateDepartmentValues
    ) => {
        setIsCreating(true);
        setError(null);

        try {
            await apiClient.post(
                API_ROUTES.DEPARTMENTS.CREATE,
                {
                    name: values.name.trim(),
                    code: values.code.trim().toUpperCase(),
                    description:
                        values.description.trim() || null,
                    managerId: values.managerId
                        ? Number(values.managerId)
                        : null
                }
            );

            onCreated();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "Could not create the department.");
        } finally {
            setIsCreating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Create department"
            centered
        >
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <Stack>
                    {error && (
                        <Alert color="red" title="Creation failed">
                            {error}
                        </Alert>
                    )}

                    <TextInput
                        withAsterisk
                        label="Name"
                        placeholder="Human Resources"
                        disabled={isCreating}
                        {...form.getInputProps("name")}
                    />

                    <TextInput
                        withAsterisk
                        label="Code"
                        placeholder="HR"
                        disabled={isCreating}
                        {...form.getInputProps("code")}
                    />

                    <Textarea
                        label="Description"
                        placeholder="Department description"
                        minRows={3}
                        disabled={isCreating}
                        {...form.getInputProps("description")}
                    />

                    <EmployeeSelect
                        label="Manager"
                        {...form.getInputProps("managerId")}
                    />

                    <Group justify="flex-end">
                        <Button
                            type="button"
                            variant="default"
                            onClick={handleClose}
                            disabled={isCreating}
                        >
                            Cancel
                        </Button>

                        <Button type="submit" loading={isCreating}>
                            Create department
                        </Button>
                    </Group>
                </Stack>
            </form>
        </Modal>
    );
}
