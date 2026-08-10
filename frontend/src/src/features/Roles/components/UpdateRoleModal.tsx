import { useEffect, useState } from "react";
import {
  Alert,
  Center,
  Loader,
  Modal,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import axios from "axios";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import type { PermissionOption } from "../types/PermissionOption";
import type { RoleDetails } from "../types/RoleDetails";
import type { RoleFormValues } from "../types/RoleFormValues";
import { validateRoleForm } from "../utils/validateRoleForm";
import { RoleForm } from "./RoleForm";

interface UpdateRoleModalProps {
  opened: boolean;
  roleId: number | null;
  onClose: () => void;
  onUpdated: () => void;
}

export function UpdateRoleModal({
  opened,
  roleId,
  onClose,
  onUpdated,
}: UpdateRoleModalProps) {
  const [permissions, setPermissions] =
    useState<PermissionOption[]>([]);

  const [isLoading, setIsLoading] = useState(false);
  const [isUpdating, setIsUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<RoleFormValues>({
    initialValues: {
      name: "",
      permissionIds: [],
    },
    validate: validateRoleForm,
  });

  useEffect(() => {
    if (!opened || roleId === null) {
      return;
    }

    let cancelled = false;

    const fetchUpdateData = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const [permissionsResponse, roleResponse] =
          await Promise.all([
            apiClient.get<PermissionOption[]>(
              API_ROUTES.ROLES.GET_PERMISSIONS,
            ),

            apiClient.get<RoleDetails>(
              API_ROUTES.ROLES.GET_BY_ID(roleId),
            ),
          ]);

        if (cancelled) {
          return;
        }

        setPermissions(permissionsResponse.data);

        form.setValues({
          name: roleResponse.data.name,
          permissionIds: roleResponse.data.permissionIds,
        });

        form.clearErrors();
      } catch (requestError) {
        if (cancelled) {
          return;
        }

        const message = axios.isAxiosError(requestError)
          ? requestError.response?.data?.errors?.[0]?.description
          : null;

        setError(message ?? "Could not load the role.");
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void fetchUpdateData();

    return () => {
      cancelled = true;
    };
  }, [opened, roleId]);

  const handleClose = () => {
    form.reset();
    setError(null);
    onClose();
  };

  const handleSubmit = async (values: RoleFormValues) => {
    if (roleId === null) {
      return;
    }

    setIsUpdating(true);
    setError(null);

    try {
      await apiClient.put(
        API_ROUTES.ROLES.UPDATE(roleId),
        {
          name: values.name.trim(),
          permissionIds: values.permissionIds,
        },
      );

      onUpdated();
      handleClose();
    } catch (requestError) {
      const message = axios.isAxiosError(requestError)
        ? requestError.response?.data?.errors?.[0]?.description
        : null;

      setError(message ?? "Could not update the role.");
    } finally {
      setIsUpdating(false);
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={handleClose}
      title="Update role"
      size="lg"
      centered
      closeOnClickOutside={!isUpdating}
      closeOnEscape={!isUpdating}
    >
      {error && (
        <Alert color="red" title="Update failed" mb="md">
          {error}
        </Alert>
      )}

      {isLoading ? (
        <Center py="xl">
          <Loader />
        </Center>
      ) : (
        <RoleForm
          form={form}
          permissions={permissions}
          isSubmitting={isUpdating}
          submitLabel="Save changes"
          onSubmit={handleSubmit}
          onCancel={handleClose}
        />
      )}
    </Modal>
  );
}