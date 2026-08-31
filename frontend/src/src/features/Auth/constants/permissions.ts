export const PERMISSIONS = {
    DEPARTMENTS: {
        VIEW: "departments.view",
        CREATE: "departments.create",
        UPDATE: "departments.update",
        DELETE: "departments.delete",
    },
    POSITIONS: {
        VIEW: "positions.view",
        CREATE: "positions.create",
        UPDATE: "positions.update",
        DELETE: "positions.delete",
    },
    EMPLOYEES: {
        VIEW: "employees.view",
        CREATE: "employees.create",
        UPDATE: "employees.update",
        DELETE: "employees.delete",
        ViewSensitiveData: "employees.sensitive-data.view",
    },
    ATTENDANCE: {
        CLOCK_IN: "attendance.clockIn",
        CLOCK_OUT: "attendance.clockOut",
        VIEW: "attendance.view",
    },
    ATTENDANCE_CORRECTIONS: {
        VIEW: "attendance_corrections.view",
        APPROVE_AND_REJECT: "attendance_corrections.aprroveAndReject",
        UPDATE: "attendance_corrections.update",
        DELETE: "attendance_corrections.delete",
        SUBMIT: "attendance_corrections.submit",
    },
    LEAVE_REQUESTS: {
        APPROVE_AND_REJECT: "leaves_leaveRequest.ApproveAndReject",
        VIEW: "leaves_leaveRequest.View",
        SUBMIT: "leaves_leaveRequest.Submit",
    },
    LEAVE_TYPES: {
        CREATE: "leaves_leaveTypes.Create",
        VIEW: "leaves_leaveTypes.View",
        UPDATE: "leaves_leaveTypes.Update",
    },
    ROLES: {
        CREATE: "roles.create",
        UPDATE: "roles.update",
        VIEW: "roles.view",
    },
    PERMISSIONS: {
        VIEW: "permissions.view",
    },
    WORK_SCHEDULES: {
        MANAGE: "workSchedules.manage",
    }
} as const;