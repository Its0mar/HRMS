export const API_ROUTES = {
    AUTH : {
        LOGIN : "/Auth/login",
        REGISTER : "/Auth/organizations",
        REFRESH : "/Auth/refresh",
        LOGOUT : "/Auth/logout",
        REGISTER_EMPLOYEE : "/Auth/employees"
    },
    DEPARTMENTS : {
        "GET_ALL" : "/Departments",
        "UPDATE" : "/Departments/update",
        "CREATE" : "/Departments/create"
    },
    EMPLOYEES: {
        "GET_ALL" : "/Employees",
        "GET_OPTIONS" : "/Employees/options",
        GET_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`,
        UPDATE_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`
    },
    WORK_SCHEDULES : {
        "GET_ALL" : "/WorkSchedules",
        "CREATE" : "/WorkSchedules",
        GET_BY_ID:(scheduleId: number) => "/WorkSchedules/" + scheduleId,
        UPDATE:  "/WorkSchedules"
    },
    ROLES : {
          GET_ALL: "/Roles",
          GET_OPTIONS: "/Roles/options",
        GET_PERMISSIONS: "/Roles/permissions",
        CREATE: "/Roles",

        GET_BY_ID: (roleId: number) =>
            `/Roles/${roleId}`,

        UPDATE: (roleId: number) =>
            `/Roles/${roleId}`,
    }
}
