import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom"
import { LoginForm } from "./features/Auth/components/LoginForm"
import { DepartmentsList } from "./features/Departments/components/DepartmentsList"
import { OrganizationRegisterForm } from "./features/Auth/components/OrganizationRegisterForm"
import { MantineProvider } from '@mantine/core';

import '@mantine/core/styles.css';
import { HeaderMegaMenu } from "./Common/HeaderMegaMenu/HeaderMegaMenu";
import { ProtectedRoute } from "./Components/ProtectedRoute";
import { PublicRoute } from "./Components/PublicRoute";
import { EmployeesList } from "./features/Employees/components/EmployeesList";
import { WorkSchedules } from "./features/WorkSchedules/components/WorkSchedules";
import { RolesList } from "./features/Roles/components/RolesList";
import { PermissionRoute } from "./Components/PermissionRoute";
import { PERMISSIONS } from "./features/Auth/constants/permissions";
import { BasicInfo } from "./features/Dashboard/components/BasicInfo";
import { AttendanceList } from "./features/Attendance/components/AttendanceList";

function App() {
  return (
    <MantineProvider>
      <BrowserRouter>
        <HeaderMegaMenu/>
        <div className="min-h-screen bg-gray-900 text-white">
          <Routes>
            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<Navigate to="/dashboard" replace />} />
              <Route path="/dashboard" element={<BasicInfo />} />


              {/* <PermissionRoute permission={PERMISSIONS.DEPARTMENTS.VIEW}> */}
                <Route path="/departments" element={ 
                  <PermissionRoute permission={PERMISSIONS.DEPARTMENTS.VIEW}>
                    <DepartmentsList /> 
                  </PermissionRoute>}
                  />
              

          
                <Route path="/employees" element={
                  <PermissionRoute permission={PERMISSIONS.EMPLOYEES.VIEW}>
                    <EmployeesList />
                    </PermissionRoute>}
                  />
              


              <Route path="/work-schedules" element={<WorkSchedules />} />
              <Route path="/roles" element={<RolesList/>} />
              <Route path="attendances" element={<AttendanceList />} />
            </Route>

            <Route element={<PublicRoute />}>
              <Route path="/register" element={<OrganizationRegisterForm />} />
              <Route path="/login" element={<LoginForm />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </div>
      </BrowserRouter>
    </MantineProvider>
  )
}

export default App
