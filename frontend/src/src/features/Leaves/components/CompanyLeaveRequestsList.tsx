import { useEffect, useState } from "react";
import type { CompanyLeaveRequestItem } from "../types/CompanyLeaveRequestItem";
import { getCompanyLeaveRequestsColumns } from "./CompanyLeaveRequestsColumns";
import { RejectLeaveModal } from "./RejectLeaveModal";
import { DataTable } from "../../../Common/DataTable/DataTable";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";
import { useDisclosure } from "@mantine/hooks";
import { notifications } from "@mantine/notifications";
import {
    Alert,
    Badge,
    Button,
    Group,
    SegmentedControl,
    Stack,
    Text,
    ThemeIcon,
    Title
} from "@mantine/core";
import { IconBuildingCommunity, IconRefresh } from "@tabler/icons-react";

import { ViewLeaveDetailsModal } from "./ViewLeaveDetailsModal";

export function CompanyLeaveRequestsList() {
    const [requests, setRequests] = useState<CompanyLeaveRequestItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Status Filter: 1 = Pending, 2 = Approved, 3 = Rejected, 0 = All
    const [statusFilter, setStatusFilter] = useState<string>("1");

    // Reject Modal State
    const [rejectModalOpened, rejectModal] = useDisclosure(false);
    const [viewModalOpened, viewModal] = useDisclosure(false);
    const [selectedRequest, setSelectedRequest] = useState<CompanyLeaveRequestItem | null>(null);
    const [viewingRequest, setViewingRequest] = useState<CompanyLeaveRequestItem | null>(null);

    const fetchLeaveRequests = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const params: Record<string, string | number> = {};
            if (statusFilter !== "0") {
                params.status = Number(statusFilter);
            }

            const response = await apiClient.get<CompanyLeaveRequestItem[]>(
                API_ROUTES.LEAVES.GET_ORGANIZATION_REQUESTS,
                { params }
            );

            setRequests(response.data);
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "Unable to load company leave requests.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void fetchLeaveRequests();
    }, [statusFilter]);

    // Handle Approve Action
    const handleApprove = async (item: CompanyLeaveRequestItem) => {
        setIsSubmitting(true);
        try {
            await apiClient.post(API_ROUTES.LEAVES.APPROVE_REQUEST, {
                requestId: item.id
            });

            notifications.show({
                title: "Request Approved",
                message: `Leave request for ${item.employeeName} approved and attendance synced!`,
                color: "green"
            });

            await fetchLeaveRequests();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : "Failed to approve leave request.";
            notifications.show({ title: "Error", message, color: "red" });
        } finally {
            setIsSubmitting(false);
        }
    };

    // Handle Reject Action Trigger
    const handleOpenRejectModal = (item: CompanyLeaveRequestItem) => {
        setSelectedRequest(item);
        rejectModal.open();
    };

    // Handle Reject Action Confirm
    const handleConfirmReject = async (rejectionReason: string) => {
        if (!selectedRequest) return;
        setIsSubmitting(true);
        try {
            await apiClient.post(API_ROUTES.LEAVES.REJECT_REQUEST, {
                requestId: selectedRequest.id,
                rejectionReason
            });

            notifications.show({
                title: "Request Rejected",
                message: `Leave request for ${selectedRequest.employeeName} has been rejected.`,
                color: "red"
            });

            await fetchLeaveRequests();
        } catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : "Failed to reject leave request.";
            notifications.show({ title: "Error", message, color: "red" });
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleViewDetails = (item: CompanyLeaveRequestItem) => {
        setViewingRequest(item);
        viewModal.open();
    };

    const columns = getCompanyLeaveRequestsColumns(handleApprove, handleOpenRejectModal, handleViewDetails, isSubmitting);

    return (
        <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
            <Stack gap="xl">
                {/* Header */}
                <Group justify="space-between" align="flex-end">
                    <div>
                        <Group gap="sm" mb={6}>
                            <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                <IconBuildingCommunity size={22} />
                            </ThemeIcon>
                            <Title order={1}>Leave Requests Approval</Title>
                        </Group>
                        <Text c="gray.4">Review and manage company employee leave applications.</Text>
                    </div>

                    {/* Status Tabs Switcher */}
                    <SegmentedControl
                        value={statusFilter}
                        onChange={setStatusFilter}
                        data={[
                            { label: "Pending", value: "1" },
                            { label: "Approved", value: "2" },
                            { label: "Rejected", value: "3" },
                            { label: "All", value: "0" }
                        ]}
                    />
                </Group>

                {/* Error Alert */}
                {error && (
                    <Alert color="red" title="Unable to load requests">
                        <Group justify="space-between" align="center">
                            <Text size="sm">{error}</Text>
                            <Button size="xs" variant="light" color="red" leftSection={<IconRefresh size={15} />} onClick={fetchLeaveRequests}>
                                Retry
                            </Button>
                        </Group>
                    </Alert>
                )}

                {/* Data Table */}
                <DataTable
                    data={requests}
                    columns={columns}
                    getRowKey={(item) => item.id}
                    isLoading={isLoading}
                    minWidth={950}
                    emptyTitle="No leave requests found"
                    emptyDescription="No leave requests match the selected status filter."
                />

                {/* Rejection Modal */}
                <RejectLeaveModal
                    opened={rejectModalOpened}
                    onClose={rejectModal.close}
                    onConfirm={handleConfirmReject}
                    isSubmitting={isSubmitting}
                />

                {/* Leave Details Modal */}
                <ViewLeaveDetailsModal
                    opened={viewModalOpened}
                    onClose={viewModal.close}
                    request={viewingRequest}
                />
            </Stack>
        </main>
    );
}