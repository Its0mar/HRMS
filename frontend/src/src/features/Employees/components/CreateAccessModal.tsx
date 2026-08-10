import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import type { CreateAccessFormValues } from "../types/CreateAccessFormValues";
import { validateCreateAccessForm } from "../utils/validateCreateAccessForm";
import { apiClient } from "../../../lib/apiClient";
import type { RoleSelectOption } from "../types/RoleSelectOption";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Alert, Modal, Stack, Text } from "@mantine/core";
import { EmployeeAccessForm } from "./EmployeeAccessForm";

interface CreateAccessModalProps {
    opened: boolean;
    employeeName: string | null;
    employeeId: number | null;
    onClose: () => void;
    onCreated: () => void;
}

export function CreateAccessModal({ opened, employeeName, employeeId, onClose, onCreated }: CreateAccessModalProps) {

    const [isCreating, setIsCreating] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isLoadingRoles, setIsLoadingRoles] = useState(false);
    const [roleOptions, setRoleOptions] = useState<RoleSelectOption[]>([]);

    const form = useForm<CreateAccessFormValues>({
        "initialValues": {
            "username": "",
            "roleId": "",
            "password": "",
            "confirmPassword": ""
        },
        validate: validateCreateAccessForm
    });

    useEffect(() => {
        if (!opened) {
            return;
        }

        let cancelled = false;

        const fetchRoles = async () => {
            setIsLoadingRoles(true);
            setError(null);

            try {
                const response = await apiClient.get<
                    Array<{
                        id: number;
                        name: string;
                    }>
                >(API_ROUTES.ROLES.GET_OPTIONS);

                if (cancelled) {
                    return;
                }

                const options = response.data.map((role) => ({
                    value: role.id.toString(),
                    label: role.name,
                }));

                setRoleOptions(options);
            } catch (requestError) {
                if (cancelled) {
                    return;
                }

                const message = axios.isAxiosError(requestError)
                    ? requestError.response?.data?.errors?.[0]?.description
                    : null;

                setError(message ?? "Could not load roles.");
            } finally {
                if (!cancelled) {
                    setIsLoadingRoles(false);
                }
            }
        };

        void fetchRoles();

        return () => {
            cancelled = true;
        };
    }, [opened]);

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (
        values: CreateAccessFormValues,
    ) => {
        if (employeeId === null || values.roleId === null) {
            return;
        }

        setIsCreating(true);
        setError(null);

        try {
            await apiClient.post(
                API_ROUTES.AUTH.REGISTER_EMPLOYEE,
                {
                    employeeId,
                    userName: values.username.trim(),
                    roleId: Number(values.roleId),
                    password: values.password,
                    confirmPassword: values.confirmPassword,
                },
            );

            onCreated();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(
                message ?? "Could not create access for this employee.",
            );
        } finally {
            setIsCreating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Create employee access"
            centered
            size="md"
            closeOnClickOutside={!isCreating}
            closeOnEscape={!isCreating}
        >
            <Stack gap="md">
                {employeeName && (
                    <div>
                        <Text size="sm" c="dimmed">
                            Creating access for
                        </Text>

                        <Text fw={600}>
                            {employeeName}
                        </Text>
                    </div>
                )}

                {error && (
                    <Alert color="red" title="Unable to create access">
                        {error}
                    </Alert>
                )}

                <EmployeeAccessForm
                    form={form}
                    roleOptions={roleOptions}
                    isSubmitting={isCreating}
                    isLoadingRoles={isLoadingRoles}
                    submitLabel="Create access"
                    showPasswordFields
                    onSubmit={handleSubmit}
                    onCancel={handleClose}
                />
                
            </Stack>
        </Modal>
    );
}


