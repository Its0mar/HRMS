import { useEffect, useState } from "react";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";

interface Deparment {
    id: number;
    name: string;
}

export function DepartmentsList() {
    
    const [departments, setDepartments] = useState<Deparment[]>([]);
    const [isLoading, setIsLoading] = useState(false);

    const fetchDepartments = async () => {
        setIsLoading(true);
        try {
            const response = await apiClient.get(API_ROUTES.DEPARTMENTS.GET_ALL);
            setDepartments(response.data);
        } catch (error : any) {
            console.error("Failed to fetch departments:", error);
            console.log(error.response?.status);
            console.log(error.response?.data);
            console.log(error.config);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchDepartments();
    }, []);

    return (
        <div>
            Departments
            {isLoading && <div>Loading...</div>}
            {departments.map((department) => (
                <div key={department.id}>{department.name}</div>
            ))}
        </div>
    )

}