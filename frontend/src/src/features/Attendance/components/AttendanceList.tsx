import { useEffect, useState } from "react";
import type { AttendanceListItem } from "../types/AttendanceListItem";
import { DataTable } from "../../../Common/DataTable/DataTable";
import { apiClient } from "../../../lib/apiClient";
import axios from "axios";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { Alert, Text, Button, Group, Stack, ThemeIcon, Title } from "@mantine/core";
import { IconRefresh, IconSettings } from "@tabler/icons-react";
import { ClockWidget } from "./ClockWidget";
import { useDisclosure } from "@mantine/hooks";
import { SubmitCorrectionModal } from "./SubmitCorrectionModal";
import { getAttendanceColumns } from "./AttendanceColumns";

export function AttendanceList() {
    const [attendances, setAttendances] = useState<AttendanceListItem[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [submitCorrectionOpened, submitCorrectionModel] = useDisclosure(false);
    const [selectedLogId, setselectedLogId] = useState<number | null>(null);

    const handleUpdateTimingRow = (attendance: AttendanceListItem) => {
        setselectedLogId(attendance.id);
        submitCorrectionModel.open();
    };

    const handleSubmitClose = () => {
        setselectedLogId(null);
        submitCorrectionModel.close();
    }
    

    const fetchAttendances = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await apiClient.get<AttendanceListItem[]>(API_ROUTES.ATTENDANCES.GET_ALL);
            setAttendances(response.data);
        }
        catch (err) {
            const message = axios.isAxiosError(err)
                ? err.response?.data?.errors?.[0]?.description
                : null;
            setError(message ?? "We could not load the attendances.");
        }
        finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void fetchAttendances();
    }, []);

    const todayDateStr = new Date().toISOString().split("T")[0];
    const todayRecord = attendances.find((a) => a.date.toString() === todayDateStr) || null;

    return (
       <main className="mx-auto w-full max-w-6xl px-4 py-10 sm:px-6">
                   <Stack gap="xl">
                       <Group justify="space-between" align="flex-end">
                           <div>
                               <Group gap="sm" mb={6}>
                                   <ThemeIcon size={38} radius="md" color="indigo" variant="light">
                                       <IconSettings size={22} />
                                   </ThemeIcon>
       
                                   <Title order={1}>Attendances</Title>
       
                               </Group>
                               
                               <Text c="gray.4">
                                   View your attendance records.
                               </Text>
                           </div>
                       </Group>
       
                       {error && (
                           <Alert
                               color="red"
                               title="Unable to load attendances">
       
                               <Group justify="space-between" align="center">
                                   <Text size="sm">{error}</Text>
                                   <Button
                                       size="xs"
                                       variant="light"
                                       color="red"
                                       leftSection={<IconRefresh size={15} />}
                                       onClick={fetchAttendances}
                                   >
                                       Retry
                                   </Button>
                               </Group>        
                           </Alert>
                       )}
       
            <ClockWidget todayRecord={todayRecord} onPunchSuccess={fetchAttendances} />

            <DataTable
                data={attendances}
                columns={getAttendanceColumns(handleUpdateTimingRow)}
                getRowKey={(attendance) => attendance.id}
                isLoading={isLoading}
                minWidth={1000}
                emptyTitle="No attendances yet"
                emptyDescription="attendances will appear here once created."
            />

            <SubmitCorrectionModal
                opened={submitCorrectionOpened}
                onClose={handleSubmitClose}
                onCreated={fetchAttendances}
                attendanceLogId={selectedLogId}
            />

            </Stack>
        </main>
    )
}