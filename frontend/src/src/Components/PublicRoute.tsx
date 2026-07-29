import { useAuthStore } from "../store/useAuthStore";
import { Navigate, Outlet } from "react-router-dom";


export function PublicRoute() {
    const isAuthenticated = useAuthStore((state) => Boolean(state.accessToken));

    if (isAuthenticated) {
        return <Navigate to="/departments" replace />;
    }

    return <Outlet />
}