export const API_ROUTES = {
    AUTH : {
        LOGIN : "/Auth/login",
        REFRESH : "/Auth/refresh",
        LOGOUT : "/Auth/logout",
        REGISTER_EMPLOYEE : "/Auth/employees"
    },
    ORGRANIZATIONS : {
        CREATE : "/Organizations",
        
    },
    DEPARTMENTS : {
        "GET_ALL" : "/Departments",
        "UPDATE" : "/Departments/update",
        "CREATE" : "/Departments/create"
    },
    EMPLOYEES: {
        "CREATE": "/Employees",
        "GET_ALL" : "/Employees",
        "GET_OPTIONS" : "/Employees/options",
        GET_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`,
        UPDATE_ACCESS: (employeeId: number) => `/Employees/${employeeId}/access`
    },
    POSITIONS: {
        "GET_ALL": "/Positions"
    },
    WORK_SCHEDULES : {
        "GET_ALL" : "/WorkSchedules",
        "CREATE" : "/WorkSchedules",
        GET_BY_ID:(scheduleId: number) => "/WorkSchedules/" + scheduleId,
        UPDATE:  "/WorkSchedules",
        "GET_OPTIONS" : "/WorkSchedules/options",
        ASSIGN_EMPLOYEE: "/WorkSchedules/assignments",
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
