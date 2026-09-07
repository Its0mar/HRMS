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
        GET_ALL: "/Departments",
        UPDATE: "/Departments",
        CREATE: "/Departments"
    },
    EMPLOYEES: {
        CREATE: "/Employees",
        GET_ALL: "/Employees",
        GET_OPTIONS: "/Employees/options",
        GET_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`,
        UPDATE_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`
    },
    POSITIONS: {
        GET_ALL: "/Positions",
        CREATE: "/Positions"
    },
    WORK_SCHEDULES: {
        GET_ALL: "/WorkSchedules",
        CREATE: "/WorkSchedules",
        GET_BY_ID: (scheduleId: number) => "/WorkSchedules/" + scheduleId,
        UPDATE: "/WorkSchedules",
        GET_OPTIONS: "/WorkSchedules/options",
        ASSIGN_EMPLOYEE: "/WorkSchedules/assignments",
    },
    ROLES: {
        GET_ALL: "/Roles",
        GET_OPTIONS: "/Roles/options",
        GET_PERMISSIONS: "/Roles/permissions",
        CREATE: "/Roles",
        GET_BY_ID: (roleId: number) => `/Roles/${roleId}`,
        UPDATE: (roleId: number) => `/Roles/${roleId}`,
    },
    ATTENDANCES: {
        GET_ALL: "/Attendances",
        CLOCK_IN: "/Attendances/clock-in",
        CLOCK_OUT: "/Attendances/clock-out",
        SUBMIT_CORRECTION: "/Attendances/corrections",
        GET_ORGANIZATION: "/Attendances/organization",
        CORRECTIONS: {
            GET_ALL: "/Attendances/corrections/organization",
            Approve_Reject: "/Attendances/corrections/approve"
        }
    },
    LEAVES: {
        GET_ALL: "/Leaves/types",
        CREATE: "/Leaves/types",
        UPDATE: "/Leaves/types",
        GET_MY_BALANCES: "/Leaves/balances",
        APPLY: "/Leaves/requests",
        GET_MY_REQUESTS: "/Leaves/requests/my",
        GET_ORGANIZATION_REQUESTS: "/Leaves/requests/organization",
        APPROVE_REQUEST: "/Leaves/requests/approve",
        REJECT_REQUEST: "/Leaves/requests/reject",
        CANCEL_REQUEST: (id: number) => `/Leaves/requests/${id}/cancel`,
    },
    DASHBOARD: {
        GET_EMPLOYEE: "/Dashboards/employee",
        GET_ADMIN: "/Dashboards/admin"
    }
};
