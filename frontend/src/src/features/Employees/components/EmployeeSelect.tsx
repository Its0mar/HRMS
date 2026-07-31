import { useEffect } from "react";
import {
    Loader,
    Select,
    type SelectProps
} from "@mantine/core";

import { useEmployeeOptionsStore } from "../store/useEmployeeOptionsStore";

type EmployeeSelectProps = Omit<SelectProps, "data">;

export function EmployeeSelect({
    disabled,
    rightSection,
    ...props
}: EmployeeSelectProps) {
    const employees = useEmployeeOptionsStore(
        (state) => state.employees
    );

    const isLoading = useEmployeeOptionsStore(
        (state) => state.isLoading
    );

    const error = useEmployeeOptionsStore(
        (state) => state.error
    );

    const loadEmployees = useEmployeeOptionsStore(
        (state) => state.loadEmployees
    );

    useEffect(() => {
        void loadEmployees();
    }, [loadEmployees]);

    const options = employees.map((employee) => ({
        value: employee.id.toString(),
        label: `${employee.fullName} (${employee.employeeNumber})`
    }));

    return (
        <Select
            searchable
            clearable
            nothingFoundMessage="No employees found"
            placeholder="Select an employee"
            {...props}
            data={options}
            disabled={disabled || isLoading}
            error={error}
            rightSection={
                isLoading
                    ? <Loader size={16} />
                    : rightSection
            }
        />
    );
}
