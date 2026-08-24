export interface AttendanceCorrectionListItem {
    id : number;
    attendanceLogId : number | null;
    requestedClockIn : string;
    requestedClockOut : string;
    status : number;
    reason : string;
    employeeNumber : string;
    employeeName : string;

}
