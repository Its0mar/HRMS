import { useState } from "react";
import { Alert, Button, Group, Modal, Stack, Textarea } from "@mantine/core";

interface RejectLeaveModalProps {
    opened: boolean;
    onClose: () => void;
    onConfirm: (reason: string) => Promise<void>;
    isSubmitting: boolean;
}

export function RejectLeaveModal({ opened, onClose, onConfirm, isSubmitting }: RejectLeaveModalProps) {
    const [reason, setReason] = useState("");
    const [error, setError] = useState<string | null>(null);

    const handleClose = () => {
        setReason("");
        setError(null);
        onClose();
    };

    const handleSubmit = async () => {
        if (!reason.trim()) {
            setError("Please provide a reason for rejecting this leave request.");
            return;
        }

        setError(null);
        await onConfirm(reason.trim());
        handleClose();
    };

    return (
        <Modal opened={opened} onClose={handleClose} title="Reject Leave Request" centered size="md">
            <Stack gap="md">
                {error && <Alert color="red">{error}</Alert>}

                <Textarea
                    label="Rejection Reason"
                    placeholder="Describe why this leave request is being rejected..."
                    withAsterisk
                    minRows={3}
                    value={reason}
                    onChange={(e) => setReason(e.target.value)}
                    disabled={isSubmitting}
                />

                <Group justify="flex-end">
                    <Button variant="light" color="gray" onClick={handleClose} disabled={isSubmitting}>
                        Cancel
                    </Button>
                    <Button color="red" loading={isSubmitting} onClick={handleSubmit}>
                        Reject Request
                    </Button>
                </Group>
            </Stack>
        </Modal>
    );
}