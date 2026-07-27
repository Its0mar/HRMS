import { BrowserRouter, Route, Routes } from "react-router-dom"
import { LoginForm } from "./features/Auth/components/LoginForm"
import { DepartmentsList } from "./features/Auth/components/DepartmentsList"
import { OrganizationRegisterForm } from "./features/Auth/components/OrganizationRegisterForm"
// import { OrganizationRegisterForm } from "./features/Auth/components/OrganizationRegisterForm"

function App() {
  return (
    <div className="min-h-screen bg-gray-900 text-white">
      <BrowserRouter>
      <Routes>
        <Route path="/departments" element={<DepartmentsList />} />
        <Route path="/register" element={<OrganizationRegisterForm />} />
        <Route path="/login" element={<LoginForm />} />
      </Routes>
      </BrowserRouter>
    </div>
  )
}

export default App
