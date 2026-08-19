import { Alert, Modal } from "@mantine/core";
import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import type { SubmitCorrectionFormValue } from "../types/SubmitCorrectionFormValue";
import { validateSubmitCorrectionForm } from "../utils/validateSubmitCorrectionForm";
import { SubmitCorrectionForm } from "./SubmitCorrectionForm";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";

interface SubmitCorrectionModalProps {
    opened: boolean;
    onClose: () => void;
    onCreated: () => void;
    attendanceLogId?: number | null;
}

export function SubmitCorrectionModal({ opened, onClose, onCreated, attendanceLogId = null }: SubmitCorrectionModalProps) {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<SubmitCorrectionFormValue>({
        initialValues: {
            attendanceLogId: attendanceLogId,
            requestedClockIn: "",
            requestedClockOut: "",
            reason: ""
        },
        validate: validateSubmitCorrectionForm
    });

    const handleClose = () => {
        form.reset();
        setError(null);
        onClose();
    };

    const handleSubmit = async (values: SubmitCorrectionFormValue) => {
        setIsSubmitting(true);
        setError(null);

        try {
            const payload = {
                attendanceLogId: attendanceLogId,
                requestedClockIn: new Date(values.requestedClockIn).toISOString(),
                requestedClockOut: new Date(values.requestedClockOut).toISOString(),
                reason: values.reason.trim()
            };

            await apiClient.post(API_ROUTES.ATTENDANCES.SUBMIT_CORRECTION, payload);
            
            handleClose();
            onCreated();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "Unable to submit correction request.");
        } finally {
            setIsSubmitting(false);
        }
    };

    useEffect(() => {
        if (opened) {
            form.setFieldValue("attendanceLogId", attendanceLogId);
        }
    }, [opened, attendanceLogId]);

    return (
        <Modal
            opened={opened}
            onClose={handleClose}
            title="Request Attendance Correction"
            size="lg"
            centered
            closeOnClickOutside={!isSubmitting}
            closeOnEscape={!isSubmitting}
        >
            {error && (
                <Alert color="red" title="Submission failed" mb="md">
                    {error}
                </Alert>
            )}

            <SubmitCorrectionForm
                form={form}
                isSubmitting={isSubmitting}
                onCancel={handleClose}
                onSubmit={handleSubmit}
            />
        </Modal>
    );
}