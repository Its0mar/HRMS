import { useState } from "react";
import { Modal, PasswordInput, Button, Stack, Group, Text, Alert } from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { IconLock, IconKey } from "@tabler/icons-react";

interface Props {
    opened: boolean;
    onClose: () => void;
}

export function ChangePasswordModal({ opened, onClose }: Props) {
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleClose = () => {
        setOldPassword("");
        setNewPassword("");
        setConfirmPassword("");
        setError(null);
        onClose();
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!oldPassword) {
            setError("Current password is required.");
            return;
        }

        if (newPassword.length < 8) {
            setError("New password must be at least 8 characters long.");
            return;
        }

        if (newPassword === oldPassword) {
            setError("New password must be different from current password.");
            return;
        }

        if (newPassword !== confirmPassword) {
            setError("New passwords do not match.");
            return;
        }

        setIsSubmitting(true);
        try {
            await apiClient.post(API_ROUTES.AUTH.CHANGE_PASSWORD, {
                oldPassword,
                newPassword
            });

            notifications.show({
                title: "Password Updated",
                message: "Your password has been changed successfully.",
                color: "green"
            });

            handleClose();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description ?? err.response?.data?.title
                : null;
            setError(message ?? "Failed to change password. Please check your current password.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Modal opened={opened} onClose={handleClose} title="Change Account Password" centered radius="md">
            <form onSubmit={handleSubmit}>
                <Stack gap="md">
                    {error && (
                        <Alert color="red" radius="md">
                            <Text size="sm">{error}</Text>
                        </Alert>
                    )}

                    <PasswordInput
                        label="Current Password"
                        placeholder="Enter your current password"
                        leftSection={<IconLock size={16} />}
                        value={oldPassword}
                        onChange={(e) => setOldPassword(e.currentTarget.value)}
                        required
                    />

                    <PasswordInput
                        label="New Password"
                        placeholder="Enter new password (min 8 chars)"
                        leftSection={<IconKey size={16} />}
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.currentTarget.value)}
                        required
                    />

                    <PasswordInput
                        label="Confirm New Password"
                        placeholder="Re-enter new password"
                        leftSection={<IconKey size={16} />}
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.currentTarget.value)}
                        required
                    />

                    <Group justify="flex-end" mt="sm">
                        <Button variant="light" color="gray" onClick={handleClose} disabled={isSubmitting}>
                            Cancel
                        </Button>
                        <Button color="indigo" type="submit" loading={isSubmitting}>
                            Update Password
                        </Button>
                    </Group>
                </Stack>
            </form>
        </Modal>
    );
}
