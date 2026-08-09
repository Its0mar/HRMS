import type { UseFormReturnType } from "@mantine/form";
import type { RoleFormValues } from "../types/RoleFormValues";
import { Button, Stack, TextInput, Text, Checkbox, Group, Paper, SimpleGrid } from "@mantine/core";
import type { PermissionOption } from "../types/PermissionOption";

interface RoleFormProps {
  form: UseFormReturnType<RoleFormValues>;
  permissions: PermissionOption[];
  isSubmitting: boolean;
  submitLabel: string;
  onSubmit: (
    values: RoleFormValues,
  ) => void | Promise<void>;
  onCancel: () => void;
}

export function RoleForm({   form,
  permissions,
  isSubmitting,
  submitLabel,
  onSubmit,
  onCancel, }: RoleFormProps) {

    const handlePermissionChange  = (
        permissionId : number,
        checked : boolean,
    ) => {
        const currentIds = form.values.permissionIds;

        if (checked) {
            form.setFieldValue("permissionIds", [...currentIds, permissionId]);
        } else {
            form.setFieldValue("permissionIds", currentIds.filter((id) => id !== permissionId));
        }        
    };


    return (
        <form onSubmit={form.onSubmit(onSubmit)}>
            <Stack gap="lg">
                <TextInput
                    label = "Role name"
                    placeholder="For example: Admin"
                    withAsterisk
                    disabled={isSubmitting}
                    {...form.getInputProps("name")}
                />

                <div>
                    <Group justify="space-between" mb="sm">
                        <div>
                            <Text fw={600} mb="sm">
                                Permissions
                            </Text>

                            <Text size="sm" c="dimmed">
                                Select what users with this role can access.
                            </Text>
                        </div>

                        <Text size="sm" c="dimmed">
                            {form.values.permissionIds.length} selected
                        </Text>
                    </Group>

                    <SimpleGrid cols={{ base: 1, sm: 2 }}>
                        {permissions.map((permission) => {
                            const isSelected =
                                form.values.permissionIds.includes(permission.id);

                            return (
                                <Paper
                                    key={permission.id}
                                    withBorder
                                    p="sm"
                                    radius="md"
                                >
                                    <Checkbox
                                        checked={isSelected}
                                        disabled={isSubmitting}
                                        onChange={(event) =>
                                            handlePermissionChange(
                                                permission.id,
                                                event.currentTarget.checked,
                                            )
                                        }
                                        label={
                                            <div>
                                                <Text size="sm" fw={500}>
                                                    {permission.code}
                                                </Text>

                                                <Text size="xs" c="dimmed">
                                                    {permission.description}
                                                </Text>
                                            </div>
                                        }
                                    />
                                </Paper>
                            );
                        })}
                    </SimpleGrid>

                    {form.errors.permissionIds && (
                        <Text size="xs" c="red" mt="xs">
                            {form.errors.permissionIds}
                        </Text>
                    )}
                </div>

                <Group justify="flex-end">
                    <Button
                        type="button"
                        variant="default"
                        onClick={onCancel}
                        disabled={isSubmitting}
                    >
                        Cancel
                    </Button>

                    <Button
                        type="submit"
                        loading={isSubmitting}
                    >
                        {submitLabel}
                    </Button>
                </Group>

            </Stack>
        </form>
    )
}