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
import type { RoleFormValues } from "../types/RoleFormValues";
import { validateRoleForm } from "../utils/validateRoleForm";
import { RoleForm } from "./RoleForm";

interface CreateRoleModalProps {
  opened: boolean;
  onClose: () => void;
  onCreated: () => void;
}

export function CreateRoleModal({
  opened,
  onClose,
  onCreated,
}: CreateRoleModalProps) {
  const [permissions, setPermissions] =
    useState<PermissionOption[]>([]);

  const [isLoadingPermissions, setIsLoadingPermissions] =
    useState(false);

  const [isCreating, setIsCreating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<RoleFormValues>({
    initialValues: {
      name: "",
      permissionIds: [],
    },
    validate: validateRoleForm,
  });

  useEffect(() => {
    if (!opened) {
      return;
    }

    let cancelled = false;

    const fetchPermissions = async () => {
      setIsLoadingPermissions(true);
      setError(null);

      try {
        const response = await apiClient.get<PermissionOption[]>(
          API_ROUTES.ROLES.GET_PERMISSIONS,
        );

        if (!cancelled) {
          setPermissions(response.data);
        }
      } catch {
        if (!cancelled) {
          setError("Could not load permissions.");
        }
      } finally {
        if (!cancelled) {
          setIsLoadingPermissions(false);
        }
      }
    };

    void fetchPermissions();

    return () => {
      cancelled = true;
    };
  }, [opened]);

  const handleClose = () => {
    form.reset();
    setError(null);
    onClose();
  };

  const handleSubmit = async (values: RoleFormValues) => {
    setIsCreating(true);
    setError(null);

    try {
      await apiClient.post(API_ROUTES.ROLES.CREATE, {
        name: values.name.trim(),
        permissionIds: values.permissionIds,
      });

      onCreated();
      handleClose();
    } catch (requestError) {
      const message = axios.isAxiosError(requestError)
        ? requestError.response?.data?.errors?.[0]?.description
        : null;

      setError(message ?? "Could not create the role.");
    } finally {
      setIsCreating(false);
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={handleClose}
      title="Create role"
      size="lg"
      centered
      closeOnClickOutside={!isCreating}
      closeOnEscape={!isCreating}
    >
      {error && (
        <Alert color="red" title="Creation failed" mb="md">
          {error}
        </Alert>
      )}

      {isLoadingPermissions ? (
        <Center py="xl">
          <Loader />
        </Center>
      ) : (
        <RoleForm
          form={form}
          permissions={permissions}
          isSubmitting={isCreating}
          submitLabel="Create role"
          onSubmit={handleSubmit}
          onCancel={handleClose}
        />
      )}
    </Modal>
  );
}