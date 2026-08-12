import type { UseFormReturnType } from "@mantine/form"
import type { CreateEmployeeFormValues } from "../types/CreateEmployeeFormValues"
import { Button, FileInput, Group, Select, SimpleGrid, Stack, TextInput } from "@mantine/core";

type SelectOption = { value: string; label: string };

interface CreateEmployeeFormProps {
    form: UseFormReturnType<CreateEmployeeFormValues>;
    departments: SelectOption[];
    positions: SelectOption[];
    employees: SelectOption[];
    isSubmitting: boolean;
    submitLabel: string;
    onSubmit: (
        values: CreateEmployeeFormValues,
    ) => void | Promise<void>;
    onCancel: () => void;
}

export function CreateEmployeeForm({
    form,
    departments,
    positions,
    employees,
    isSubmitting,
    submitLabel,
    onSubmit,
    onCancel,
}: CreateEmployeeFormProps) {

    return (
        <form onSubmit={form.onSubmit(onSubmit)}>
            <Stack gap="md">
                <SimpleGrid cols={{ base: 1, sm: 2 }}>
                <TextInput
                    label="Employee number"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("employeeNumber")}
                />

                <TextInput
                    label="First name"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("firstName")}
                />

                <TextInput
                    label="Last name"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("lastName")}
                />

                <TextInput
                    type="date"
                    label="Date of birth"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("dateOfBirth")}
                />

                <Select
                    label="Gender"
                    data={[
                        { value: "1", label: "Male" },
                        { value: "2", label: "Female" },
                    ]}
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("gender")}
                />

                <TextInput
                    label="NationalId"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("nationalId")}
                />

                <TextInput
                    label="Nationality"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("nationality")}
                />

                <Select
                    label="Marital status"
                    data={[
                        { value: "1", label: "Single" },
                        { value: "2", label: "Married" },
                        { value: "3", label: "Divorced" },
                        { value: "4", label: "Widowed" },
                        { value: "5", label: "Separated" },
                    ]}
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("maritalStatus")}
                />

                <TextInput
                    label="Phone"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("phone")}
                />

                <TextInput
                    label="Email"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("email")}
                />

                <TextInput
                    label="Address"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("address")}
                />

                <FileInput
                    label="Profile picture"
                    accept="image/png,image/jpeg,image/webp"
                    clearable
                    disabled={isSubmitting}
                    {...form.getInputProps("profilePicture")}
                />
                </SimpleGrid>

                <SimpleGrid cols={{ base: 1, sm: 2 }}>
                    <Select
                        label="Department"
                        placeholder="Select a department"
                        data={departments}
                        searchable
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("departmentId")}
                    />

                    <Select
                        label="Position"
                        placeholder="Select a position"
                        data={positions}
                        searchable
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("positionId")}
                    />

                    <Select
                        label="Manager"
                        placeholder="Select a manager"
                        data={employees}
                        searchable
                        clearable
                        disabled={isSubmitting}
                        {...form.getInputProps("managerEmployeeId")}
                    />

                    <TextInput
                        type="date"
                        label="Hire date"
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("hireDate")}
                    />

                    <Select
                        label="Employment type"
                        placeholder="Select employment type"
                        data={[
                            { value: "1", label: "Full time" },
                            { value: "2", label: "Part time" },
                            { value: "3", label: "Contract" },
                            { value: "4", label: "Intern" },
                            { value: "5", label: "Temporary" },
                        ]}
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("employmentType")}
                    />

                    <Select
                        label="Employment status"
                        placeholder="Select employment status"
                        data={[
                            { value: "1", label: "Active" },
                            { value: "2", label: "On leave" },
                            { value: "3", label: "Probation" },
                            { value: "4", label: "Resigned" },
                            { value: "5", label: "Terminated" },
                            { value: "6", label: "Retired" },
                        ]}
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("employmentStatus")}
                    />

                    <TextInput
                        type="email"
                        label="Work email"
                        placeholder="employee@company.com"
                        withAsterisk
                        disabled={isSubmitting}
                        {...form.getInputProps("workEmail")}
                    />

                    <TextInput
                        type="tel"
                        label="Work phone"
                        placeholder="+962..."
                        disabled={isSubmitting}
                        {...form.getInputProps("workPhone")}
                    />
                </SimpleGrid>


                <Group justify="flex-end">
                    <Button
                        type="button"
                        variant="default"
                        disabled={isSubmitting}
                        onClick={onCancel}
                    >
                        Cancel
                    </Button>

                    <Button type="submit" loading={isSubmitting}>
                        {submitLabel}
                    </Button>
                </Group>
            </Stack>
        </form>
    )
}
