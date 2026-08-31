export const API_ROUTES = {
    AUTH: {
        LOGIN: "/Auth/login",
        REFRESH: "/Auth/refresh",
        LOGOUT: "/Auth/logout",
        REGISTER_EMPLOYEE: "/Auth/employees",
        CHANGE_PASSWORD: "/Auth/change-password"
    },
    ORGRANIZATIONS: {
        CREATE: "/Organizations",

    },
    DEPARTMENTS: {
        "GET_ALL": "/Departments",
        "UPDATE": "/Departments/update",
        "CREATE": "/Departments/create"
    },
    EMPLOYEES: {
        "CREATE": "/Employees",
        "GET_ALL": "/Employees",
        "GET_OPTIONS": "/Employees/options",
        GET_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`,
        UPDATE_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`
    },
    POSITIONS: {
        "GET_ALL": "/Positions"
    },
    WORK_SCHEDULES: {
        "GET_ALL": "/WorkSchedules",
        "CREATE": "/WorkSchedules",
        GET_BY_ID: (scheduleId: number) => "/WorkSchedules/" + scheduleId,
        UPDATE: "/WorkSchedules",
        "GET_OPTIONS": "/WorkSchedules/options",
        ASSIGN_EMPLOYEE: "/WorkSchedules/assignments",
    },
    ROLES: {
        GET_ALL: "/Roles",
        GET_OPTIONS: "/Roles/options",
        GET_PERMISSIONS: "/Roles/permissions",
        CREATE: "/Roles",

        GET_BY_ID: (roleId: number) =>
            `/Roles/${roleId}`,

        UPDATE: (roleId: number) =>
            `/Roles/${roleId}`,
    },

    ATTENDANCES: {
        "GET_ALL": "/Attendances",
        "CLOCK_IN": "/Attendances/ClockIn",
        "CLOCK_OUT": "/Attendances/ClockOut",
        "SUBMIT_CORRECTION": "/Attendances/Correct",
        GET_ORGANIZATION: "/Attendances/Organization",

        CORRECTIONS: {
            "GET_ALL": "/Attendances/corrections/organization",
            "Approve_Reject": "/Attendances/corrections/approve"
        }
    },

    LEAVES: {
        "GET_ALL": "/Leaves/leavetypes/get",
        "CREATE": "/Leaves/leavetypes/create",
        "UPDATE": "/Leaves/leavetypes/update",
        GET_MY_BALANCES: "/Leaves/balances",
        APPLY: "/Leaves/leaveRequests/apply",
        GET_MY_REQUESTS: "/Leaves/leaveRequests/list",
        GET_ORGANIZATION_REQUESTS: "/Leaves/leaveRequests/organization/list",
        APPROVE_REQUEST: "/Leaves/requests/approve",
        REJECT_REQUEST: "/Leaves/requests/reject",
        CANCEL_REQUEST: (id: number) => `/Leaves/requests/${id}/cancel`,
    },

    DASHBOARD: {
        GET_EMPLOYEE: "/Dashboards/employee",
        GET_ADMIN: "/Dashboards/admin"
    }
}
