import { useEffect, useState } from "react";
import {
  Alert,
  Center,
  Loader,
  Modal,
  Stack,
  Text,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import axios from "axios";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import type { CreateAccessFormValues } from "../types/CreateAccessFormValues";
import type { EmployeeAccessDetails } from "../types/EmployeeAccessDetails";
import type { RoleSelectOption } from "../types/RoleSelectOption";
import { validateUpdateAccessForm } from "../utils/validateUpdateAccessForm";
import { EmployeeAccessForm } from "./EmployeeAccessForm";

interface UpdateAccessModalProps {
  opened: boolean;
  employeeId: number | null;
  employeeName: string | null;
  onClose: () => void;
  onUpdated: () => void;
}

export function UpdateAccessModal({
  opened,
  employeeId,
  employeeName,
  onClose,
  onUpdated,
}: UpdateAccessModalProps) {
  const [roleOptions, setRoleOptions] =
    useState<RoleSelectOption[]>([]);

  const [isLoading, setIsLoading] = useState(false);
  const [hasLoadedData, setHasLoadedData] = useState(false);
  const [isUpdating, setIsUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<CreateAccessFormValues>({
    initialValues: {
      username: "",
      roleId: null,
      password: "",
      confirmPassword: "",
    },
    validate: validateUpdateAccessForm,
  });

  useEffect(() => {
    if (!opened || employeeId === null) {
      return;
    }

    let cancelled = false;

    const fetchData = async () => {
      setIsLoading(true);
      setHasLoadedData(false);
      setError(null);

      try {
        const [rolesResponse, accessResponse] =
          await Promise.all([
            apiClient.get<Array<{ id: number; name: string }>>(
              API_ROUTES.ROLES.GET_OPTIONS,
            ),

            apiClient.get<EmployeeAccessDetails>(
              API_ROUTES.EMPLOYEES.GET_ACCESS(employeeId),

              
            ),
          ]);

        if (cancelled) {
          return;
        }


        setRoleOptions(
          rolesResponse.data.map((role) => ({
            value: role.id.toString(),
            label: role.name,
          })),
        );

        form.setValues({
          username: accessResponse.data.username,
          roleId: accessResponse.data.roleId.toString(),
          password: "",
          confirmPassword: "",
        });

        form.clearErrors();
        setHasLoadedData(true);
      } catch (requestError) {
        if (cancelled) {
          return;
        }

        const message = axios.isAxiosError(requestError)
          ? requestError.response?.data?.errors?.[0]?.description
          : null;

        setError(message ?? "Could not load employee access.");
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void fetchData();

    return () => {
      cancelled = true;
    };
  }, [opened, employeeId]);

  const handleClose = () => {
    form.reset();
    setError(null);
    setHasLoadedData(false);
    onClose();
  };

  const handleSubmit = async (
    values: CreateAccessFormValues,
  ) => {
    if (employeeId === null || values.roleId === null) {
      return;
    }

    setIsUpdating(true);
    setError(null);

    try {
      await apiClient.put(
        API_ROUTES.EMPLOYEES.UPDATE_ACCESS(employeeId),
        {
          username: values.username.trim(),
          roleId: Number(values.roleId),
        },
      );

      onUpdated();
      handleClose();
    } catch (requestError) {
      const message = axios.isAxiosError(requestError)
        ? requestError.response?.data?.errors?.[0]?.description
        : null;

      setError(message ?? "Could not update employee access.");
    } finally {
      setIsUpdating(false);
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={handleClose}
      title="Update employee access"
      centered
      size="md"
      closeOnClickOutside={!isUpdating}
      closeOnEscape={!isUpdating}
    >
      <Stack gap="md">
        {employeeName && (
          <div>
            <Text size="sm" c="dimmed">
              Updating access for
            </Text>

            <Text fw={600}>
              {employeeName}
            </Text>
          </div>
        )}

        {error && (
          <Alert color="red" title="Unable to update access">
            {error}
          </Alert>
        )}

        {isLoading && (
          <Center py="xl">
            <Loader />
          </Center>
        )}

        {hasLoadedData && (
          <EmployeeAccessForm
            form={form}
            roleOptions={roleOptions}
            isSubmitting={isUpdating}
            isLoadingRoles={false}
            submitLabel="Save changes"
            showPasswordFields={false}
            onSubmit={handleSubmit}
            onCancel={handleClose}
          />
        )}
      </Stack>
    </Modal>
  );
}