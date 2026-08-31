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
import { AttendanceList } from "./features/Attendance/components/AttendanceList";
import { CompanyAttendanceList } from "./features/Attendance/components/CompanyAttendanceList";
import { CompanyAttendanceCorrectionsList } from "./features/Attendance/components/CompanyAttendanceCorrectionsList";
import { LeaveTypesList } from "./features/Leaves/components/LeaveTypesList";
import { CompanyLeaveRequestsList } from "./features/Leaves/components/CompanyLeaveRequestsList";
import { MyLeavesPage } from "./features/Leaves/components/MyLeavesPage";
import { BasicInfo } from "./features/Dashboard/components/BasicInfo";

function App() {
  return (
    <MantineProvider>
      <BrowserRouter>
        <HeaderMegaMenu />
        <div className="min-h-screen bg-gray-900 text-white">
          <Routes>
            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<Navigate to="/dashboard" replace />} />
              <Route path="/dashboard" element={<BasicInfo />} />
              

              {/* Employee Routes (Self) */}
              <Route path="/attendances" element={<AttendanceList />} />
              <Route path="/leaves/my" element={<MyLeavesPage />} />
              <Route path="/my-leaves" element={<MyLeavesPage />} />

              {/* Company Supervision & Admin Routes (Permission Guarded) */}
              <Route
                path="/attendances/company"
                element={
                  <PermissionRoute permission={PERMISSIONS.ATTENDANCE.VIEW}>
                    <CompanyAttendanceList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/attendances/corrections/company"
                element={
                  <PermissionRoute permission={PERMISSIONS.ATTENDANCE_CORRECTIONS.VIEW}>
                    <CompanyAttendanceCorrectionsList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/leaves/types"
                element={
                  <PermissionRoute permission={PERMISSIONS.LEAVE_TYPES.VIEW}>
                    <LeaveTypesList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/leaves/company"
                element={
                  <PermissionRoute permission={PERMISSIONS.LEAVE_REQUESTS.VIEW}>
                    <CompanyLeaveRequestsList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/departments"
                element={
                  <PermissionRoute permission={PERMISSIONS.DEPARTMENTS.VIEW}>
                    <DepartmentsList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/employees"
                element={
                  <PermissionRoute permission={PERMISSIONS.EMPLOYEES.VIEW}>
                    <EmployeesList />
                  </PermissionRoute>
                }
              />

              <Route
                path="/work-schedules"
                element={
                  <PermissionRoute permission={PERMISSIONS.WORK_SCHEDULES.MANAGE}>
                    <WorkSchedules />
                  </PermissionRoute>
                }
              />

              <Route
                path="/roles"
                element={
                  <PermissionRoute permission={PERMISSIONS.ROLES.VIEW}>
                    <RolesList />
                  </PermissionRoute>
                }
              />
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
