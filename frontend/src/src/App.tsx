import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom"
import { LoginForm } from "./features/Auth/components/LoginForm"
import { DepartmentsList } from "./features/Departments/components/DepartmentsList"
import { OrganizationRegisterForm } from "./features/Auth/components/OrganizationRegisterForm"
import { MantineProvider } from '@mantine/core';

import '@mantine/core/styles.css';
import { HeaderMegaMenu } from "./Common/HeaderMegaMenu/HeaderMegaMenu";
import { ProtectedRoute } from "./Components/ProtectedRoute";
import { PublicRoute } from "./Components/PublicRoute";

function App() {
  return (
    <MantineProvider>
      <BrowserRouter>
        <HeaderMegaMenu/>
        <div className="min-h-screen bg-gray-900 text-white">
          <Routes>
            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<Navigate to="/departments" replace />} />
              <Route path="/departments" element={<DepartmentsList />} />
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
