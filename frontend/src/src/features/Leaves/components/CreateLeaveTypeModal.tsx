import { useState } from "react";
import type { LeaveTypeFormValues } from "../types/LeaveTypeFormValues";
import { useForm } from "@mantine/form";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Alert, Modal } from "@mantine/core";
import { LeaveTypeForm } from "./LeaveTypeForm";
import { validateLeaveTypeForm } from "../utils/validateLeaveTypeForm";

interface CreateLeaveTypeModalProps {
    opened: boolean;
    onClose: () => void;
    onCreated: () => void;
}

export function CreateLeaveTypeModal({
    opened,
    onClose,
    onCreated
}: CreateLeaveTypeModalProps) {
    const [isCreating, setIsCreating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<LeaveTypeFormValues>({
        initialValues: {
            name: "",
            code: "",
            defaultDaysPerYear: 21,
            isPaid: true,
            requiresApproval: true
        },
        validate: validateLeaveTypeForm
    });

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (values: LeaveTypeFormValues) => {
        setIsCreating(true);
        setError(null);

        try {
            await apiClient.post(API_ROUTES.LEAVES.CREATE, {
                name: values.name.trim(),
                code: values.code.trim().toUpperCase(),
                defaultDaysPerYear: values.defaultDaysPerYear,
                isPaid: values.isPaid,
                requiresApproval: values.requiresApproval
            });

            onCreated();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "Could not create the leave type.");
        } finally {
            setIsCreating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Create Leave Type"
            size="lg"
            centered
            closeOnClickOutside={!isCreating}
            closeOnEscape={!isCreating}
        >
            {error && (
                <Alert color="red" title="Creation failed" mb="md">
                    {error}
                </Alert>
            )}

            <LeaveTypeForm
                form={form}
                isSubmitting={isCreating}
                submitLabel="Create Leave Type"
                onSubmit={handleSubmit}
                onCancel={handleClose}
            />
        </Modal>
    );
}