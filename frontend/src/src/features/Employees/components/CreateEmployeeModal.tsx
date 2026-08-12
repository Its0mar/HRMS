import { useEffect, useState } from "react";
import { Alert, Center, Loader, Modal, ScrollArea } from "@mantine/core";
import { useForm, type FormErrors } from "@mantine/form";
import axios from "axios";

import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import type { Department } from "../../Departments/types/Department";
import type { EmployeeOption } from "../types/EmployeeOption";
import type { CreateEmployeeFormValues } from "../types/CreateEmployeeFormValues";
import { CreateEmployeeForm } from "./CreateEmployeeForm";

interface CreateEmployeeModalProps {
  opened: boolean;
  onClose: () => void;
  onCreated: () => void;
}

interface PositionOptionResponse {
  id: number;
  title: string;
}

const initialValues: CreateEmployeeFormValues = {
  employeeNumber: "",
  firstName: "",
  lastName: "",
  dateOfBirth: "",
  gender: "",
  nationalId: "",
  nationality: "",
  maritalStatus: "",
  phone: "",
  email: "",
  address: "",
  profilePicture: null,
  departmentId: null,
  positionId: null,
  managerEmployeeId: null,
  hireDate: "",
  employmentType: "",
  employmentStatus: "",
  workEmail: "",
  workPhone: "",
};

function validate(values: CreateEmployeeFormValues): FormErrors {
  const errors: FormErrors = {};

  if (values.employeeNumber.trim().length < 3) errors.employeeNumber = "Employee number is required.";
  if (values.firstName.trim().length < 3) errors.firstName = "First name must contain at least 3 characters.";
  if (values.lastName.trim().length < 3) errors.lastName = "Last name must contain at least 3 characters.";
  if (!values.dateOfBirth) errors.dateOfBirth = "Date of birth is required.";
  if (!values.gender) errors.gender = "Gender is required.";
  if (!values.nationalId.trim()) errors.nationalId = "National ID is required.";
  if (!values.nationality.trim()) errors.nationality = "Nationality is required.";
  if (!values.maritalStatus) errors.maritalStatus = "Marital status is required.";
  if (!values.phone.trim()) errors.phone = "Phone is required.";
  if (!/^\S+@\S+\.\S+$/.test(values.email)) errors.email = "Enter a valid email.";
  if (!values.address.trim()) errors.address = "Address is required.";
  if (!values.departmentId) errors.departmentId = "Department is required.";
  if (!values.positionId) errors.positionId = "Position is required.";
  if (!values.hireDate) errors.hireDate = "Hire date is required.";
  if (!values.employmentType) errors.employmentType = "Employment type is required.";
  if (!values.employmentStatus) errors.employmentStatus = "Employment status is required.";
  if (!/^\S+@\S+\.\S+$/.test(values.workEmail)) errors.workEmail = "Enter a valid work email.";

  if (values.profilePicture && values.profilePicture.size > 5 * 1024 * 1024) {
    errors.profilePicture = "Profile picture cannot exceed 5 MB.";
  }

  return errors;
}

export function CreateEmployeeModal({
  opened,
  onClose,
  onCreated,
}: CreateEmployeeModalProps) {
  const [departments, setDepartments] = useState<Array<{ value: string; label: string }>>([]);
  const [positions, setPositions] = useState<Array<{ value: string; label: string }>>([]);
  const [employees, setEmployees] = useState<Array<{ value: string; label: string }>>([]);
  const [isLoadingOptions, setIsLoadingOptions] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<CreateEmployeeFormValues>({
    initialValues,
    validate,
  });

  useEffect(() => {
    if (!opened) return;

    let cancelled = false;

    async function loadOptions() {
      setIsLoadingOptions(true);
      setError(null);

      try {
        const [departmentResponse, positionResponse, employeeResponse] = await Promise.all([
          apiClient.get<Department[]>(API_ROUTES.DEPARTMENTS.GET_ALL),
          apiClient.get<PositionOptionResponse[]>(API_ROUTES.POSITIONS.GET_ALL),
          apiClient.get<EmployeeOption[]>(API_ROUTES.EMPLOYEES.GET_OPTIONS),
        ]);

        if (cancelled) return;

        setDepartments(departmentResponse.data.map((item) => ({
          value: item.id.toString(),
          label: item.name,
        })));
        setPositions(positionResponse.data.map((item) => ({
          value: item.id.toString(),
          label: item.title,
        })));
        setEmployees(employeeResponse.data.map((item) => ({
          value: item.id.toString(),
          label: `${item.fullName} (${item.employeeNumber})`,
        })));
      } catch (requestError) {
        if (!cancelled) {
          const message = axios.isAxiosError(requestError)
            ? requestError.response?.data?.errors?.[0]?.description
            : null;
          setError(message ?? "Could not load employee form options.");
        }
      } finally {
        if (!cancelled) setIsLoadingOptions(false);
      }
    }

    void loadOptions();
    return () => { cancelled = true; };
  }, [opened]);

  const handleClose = () => {
    form.reset();
    setError(null);
    onClose();
  };

  const handleSubmit = async (values: CreateEmployeeFormValues) => {
    setIsCreating(true);
    setError(null);

    const data = new FormData();
    data.append("EmployeeNumber", values.employeeNumber.trim());
    data.append("FirstName", values.firstName.trim());
    data.append("LastName", values.lastName.trim());
    data.append("DateOfBirth", values.dateOfBirth);
    data.append("Gender", values.gender);
    data.append("NationalId", values.nationalId.trim());
    data.append("Nationality", values.nationality.trim());
    data.append("MaritalStatus", values.maritalStatus);
    data.append("Phone", values.phone.trim());
    data.append("Email", values.email.trim());
    data.append("Address", values.address.trim());
    data.append("DepartmentId", values.departmentId!);
    data.append("PositionId", values.positionId!);
    data.append("HireDate", values.hireDate);
    data.append("EmploymentType", values.employmentType);
    data.append("EmploymentStatus", values.employmentStatus);
    data.append("WorkEmail", values.workEmail.trim());
    data.append("WorkPhone", values.workPhone.trim());

    if (values.managerEmployeeId) data.append("ManagerEmployeeId", values.managerEmployeeId);
    if (values.profilePicture) data.append("ProfilePicture", values.profilePicture);

    try {
      await apiClient.post(API_ROUTES.EMPLOYEES.CREATE, data);
      onCreated();
      handleClose();
    } catch (requestError) {
      const message = axios.isAxiosError(requestError)
        ? requestError.response?.data?.errors?.[0]?.description
        : null;
      setError(message ?? "Could not create the employee.");
    } finally {
      setIsCreating(false);
    }
  };

  return (
    <Modal
      opened={opened}
      onClose={handleClose}
      title="Create employee"
      size="xl"
      centered
      scrollAreaComponent={ScrollArea.Autosize}
      closeOnClickOutside={!isCreating}
      closeOnEscape={!isCreating}
    >
      {error && <Alert color="red" title="Unable to create employee" mb="md">{error}</Alert>}

      {isLoadingOptions ? (
        <Center py="xl"><Loader /></Center>
      ) : (
        <CreateEmployeeForm
          form={form}
          departments={departments}
          positions={positions}
          employees={employees}
          isSubmitting={isCreating}
          submitLabel="Create employee"
          onSubmit={handleSubmit}
          onCancel={handleClose}
        />
      )}
    </Modal>
  );
}
