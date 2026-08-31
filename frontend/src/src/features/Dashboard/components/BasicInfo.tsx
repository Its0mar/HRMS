import { useIsManagement } from "../../Auth/hooks/useIsManagement";
import { AdminDashboard } from "./AdminDashboard";
import { EmployeeDashboard } from "./EmployeeDashboard";

export function BasicInfo() {
    const isManagement = useIsManagement();


                         


    if (isManagement) {
        return <AdminDashboard />;
    }
    return <EmployeeDashboard />;
}