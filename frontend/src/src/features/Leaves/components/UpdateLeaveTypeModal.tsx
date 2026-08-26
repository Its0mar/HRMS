import { useEffect, useState } from "react";
import type { LeaveTypeFormValues } from "../types/LeaveTypeFormValues";
import type { LeaveTypeListItem } from "../types/LeaveTypeListItem";
import { useForm } from "@mantine/form";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { Alert, Modal } from "@mantine/core";
import { LeaveTypeForm } from "./LeaveTypeForm";
import { validateLeaveTypeForm } from "../utils/validateLeaveTypeForm";

interface UpdateLeaveTypeModalProps {
    opened: boolean;
    leaveType: LeaveTypeListItem | null;
    onClose: () => void;
    onUpdated: () => void;
}

export function UpdateLeaveTypeModal({
    opened,
    leaveType,
    onClose,
    onUpdated
}: UpdateLeaveTypeModalProps) {
    const [isUpdating, setIsUpdating] = useState(false);
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

    useEffect(() => {
        if (opened && leaveType) {
            form.setValues({
                name: leaveType.name,
                code: leaveType.code,
                defaultDaysPerYear: leaveType.defaultDaysPerYear,
                isPaid: leaveType.isPaid,
                requiresApproval: leaveType.requiresApproval
            });
        }
    }, [opened, leaveType]);

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (values: LeaveTypeFormValues) => {
        if (!leaveType) return;

        setIsUpdating(true);
        setError(null);

        try {
            await apiClient.put(API_ROUTES.LEAVES.UPDATE, {
                id: leaveType.id,
                name: values.name.trim(),
                code: values.code.trim().toUpperCase(),
                defaultDaysPerYear: values.defaultDaysPerYear,
                isPaid: values.isPaid,
                requiresApproval: values.requiresApproval
            });

            onUpdated();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description
                : null;

            setError(message ?? "Could not update the leave type.");
        } finally {
            setIsUpdating(false);
        }
    };

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Edit Leave Type"
            size="lg"
            centered
            closeOnClickOutside={!isUpdating}
            closeOnEscape={!isUpdating}
        >
            {error && (
                <Alert color="red" title="Update failed" mb="md">
                    {error}
                </Alert>
            )}

            <LeaveTypeForm
                form={form}
                isSubmitting={isUpdating}
                submitLabel="Save Changes"
                onSubmit={handleSubmit}
                onCancel={handleClose}
            />
        </Modal>
    );
}