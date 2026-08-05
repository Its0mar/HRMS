export const API_ROUTES = {
    AUTH : {
        LOGIN : "/Auth/login",
        REGISTER : "/Auth/organizations",
        REFRESH : "/Auth/refresh",
        LOGOUT : "/Auth/logout",
    },
    DEPARTMENTS : {
        "GET_ALL" : "/Departments",
        "UPDATE" : "/Departments/update",
        "CREATE" : "/Departments/create"
    },
    EMPLOYEES: {
        "GET_ALL" : "/Employees",
        "GET_OPTIONS" : "/Employees/options"
    },
    WORK_SCHEDULES : {
        "GET_ALL" : "/WorkSchedules",
        "CREATE" : "/WorkSchedules",
        GET_BY_ID:(scheduleId: number) => "/WorkSchedules/" + scheduleId,
        UPDATE:  "/WorkSchedules"
    },
    ROLES : {
        "GET_ALL" : "/Roles"
    }
}
