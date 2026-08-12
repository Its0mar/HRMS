export const PERMISSIONS = {
    DEPARTMENTS : {
        VIEW : "departments.view",
        CREATE : "departments.create",
        UPDATE : "departments.update",
        DELETE : "departments.delete",
    },
    POSITIONS : {
        VIEW : "positions.view",
        CREATE : "positions.create",
        UPDATE : "positions.update",
        DELETE : "positions.delete",
    },
    EMPLOYEES : {
        VIEW : "employees.view",
        CREATE : "employees.create",
        UPDATE : "employees.update",
        DELETE : "employees.delete",
        ViewSensitiveData : "employees.sensitive-data.view",
    }
} as const;