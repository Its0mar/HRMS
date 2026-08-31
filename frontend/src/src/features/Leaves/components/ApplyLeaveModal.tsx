import { useState } from "react";
import type { ApplyLeaveFormValues } from "../types/ApplyLeaveFormValues";
import type { MyLeaveBalanceItem } from "../types/MyLeaveBalanceItem";
import { validateApplyLeaveForm } from "../utils/validateApplyLeaveForm";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { useForm } from "@mantine/form";
import { Alert, Button, Group, Modal, Select, Stack, TextInput, Textarea } from "@mantine/core";

interface ApplyLeaveModalProps {
    opened: boolean;
    balances: MyLeaveBalanceItem[];
    onClose: () => void;
    onSubmitted: () => void;
}

export function ApplyLeaveModal({ opened, balances, onClose, onSubmitted }: ApplyLeaveModalProps) {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<ApplyLeaveFormValues>({
        initialValues: {
            leaveTypeId: "",
            startDate: "",
            endDate: "",
            reason: ""
        },
        validate: validateApplyLeaveForm
    });

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (values: ApplyLeaveFormValues) => {
        setIsSubmitting(true);
        setError(null);

        try {
            await apiClient.post(API_ROUTES.LEAVES.APPLY, {
                leaveTypeId: Number(values.leaveTypeId),
                startDate: values.startDate,
                endDate: values.endDate,
                reason: values.reason.trim()
            });

            onSubmitted();
            handleClose();
        } catch (requestError) {
            const message = axios.isAxiosError(requestError)
                ? requestError.response?.data?.errors?.[0]?.description ?? requestError.response?.data?.title
                : null;

            setError(message ?? "Could not submit leave request.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const leaveOptions = balances.map((b) => ({
        value: String(b.leaveTypeId),
        label: `${b.leaveTypeName} (${b.remainingDays} days remaining)`
    }));

    return (
        <Modal opened={opened} onClose={handleClose} title="Apply for Leave" size="lg" centered closeOnClickOutside={!isSubmitting}>
            <form onSubmit={form.onSubmit(handleSubmit)}>
                <Stack gap="md">
                    {error && <Alert color="red" title="Submission failed">{error}</Alert>}

                    <Select
                        label="Leave Type"
                        placeholder="Select leave category"
                        data={leaveOptions}
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("leaveTypeId")}
                    />

                    <Group grow>
                        <TextInput
                            type="date"
                            label="Start Date"
                            withAsterisk
                            disabled={isSubmitting}
                            {...form.getInputProps("startDate")}
                        />

                        <TextInput
                            type="date"
                            label="End Date"
                            withAsterisk
                            disabled={isSubmitting}
                            {...form.getInputProps("endDate")}
                        />
                    </Group>

                    <Textarea
                        label="Reason for Leave"
                        placeholder="State the reason for your leave request..."
                        withAsterisk
                        minRows={3}
                        disabled={isSubmitting}
                        {...form.getInputProps("reason")}
                    />

                    <Group justify="flex-end" mt="md">
                        <Button variant="default" onClick={handleClose} disabled={isSubmitting}>
                            Cancel
                        </Button>
                        <Button type="submit" loading={isSubmitting}>
                            Submit Application
                        </Button>
                    </Group>
                </Stack>
            </form>
        </Modal>
    );
}