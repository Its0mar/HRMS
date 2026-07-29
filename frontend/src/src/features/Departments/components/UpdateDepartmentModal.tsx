import { useEffect, useState } from "react";
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
import type { Department } from "../types/Department";
import { API_ROUTES } from "../../../lib/apiRoutes";

const schema = z.object({
    name: z.string().min(3, "Name must contain at least 3 characters"),
    description: z.string().max(300)
});

type UpdateDepartmentValues = z.infer<typeof schema>;

interface UpdateDepartmentModalProps {
    department: Department | null;
    opened: boolean;
    onClose: () => void;
    onUpdated: () => void;
}

export function UpdateDepartmentModal({
    department,
    opened,
    onClose,
    onUpdated
}: UpdateDepartmentModalProps) {
    const [isUpdating, setIsUpdating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<UpdateDepartmentValues>({
        validate: zod4Resolver(schema),
        initialValues: {
            name: "",
            description: ""
        }
    });

    useEffect(() => {
        if (!department) return;

        form.setValues({
            name: department.name,
            description: department.description ?? ""
        });

        form.clearErrors();
        setError(null);
    }, [department]);

    const handleSubmit = async (values: UpdateDepartmentValues) => {
        if (!department) return;

        setIsUpdating(true);
        setError(null);

        try {
            await apiClient.put(
                API_ROUTES.DEPARTMENTS.UPDATE,
                {
                    id : department.id,
                    name: values.name.trim(),
                    // description: values.description.trim() || null,
                    // managerEmployeeId:
                    //     department.managerEmployeeId &&
                    //     department.managerEmployeeId > 0
                    //         ? department.managerEmployeeId
                    //         : null
                }
            );

            onUpdated();
            onClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "Could not update the department.");
        } finally {
            setIsUpdating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={onClose}
            title="Update department"
            centered
        >
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <Stack>
                    {error && (
                        <Alert color="red" title="Update failed">
                            {error}
                        </Alert>
                    )}

                    <TextInput
                        withAsterisk
                        label="Name"
                        placeholder="Human Resources"
                        {...form.getInputProps("name")}
                    />

                    <Textarea
                        label="Description"
                        placeholder="Department description"
                        minRows={3}
                        {...form.getInputProps("description")}
                    />

                    <Group justify="flex-end">
                        <Button
                            type="button"
                            variant="default"
                            onClick={onClose}
                            disabled={isUpdating}
                        >
                            Cancel
                        </Button>

                        <Button type="submit" loading={isUpdating}>
                            Save changes
                        </Button>
                    </Group>
                </Stack>
            </form>
        </Modal>
    );
}