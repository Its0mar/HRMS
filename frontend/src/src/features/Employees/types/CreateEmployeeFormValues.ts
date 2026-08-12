export interface CreateEmployeeFormValues {
  employeeNumber: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: string;
  nationalId: string;
  nationality: string;
  maritalStatus: string;
  phone: string;
  email: string;
  address: string;
  profilePicture: File | null;

  departmentId: string | null;
  positionId: string | null;
  managerEmployeeId: string | null;
  hireDate: string;
  employmentType: string;
  employmentStatus: string;
  workEmail: string;
  workPhone: string;
}