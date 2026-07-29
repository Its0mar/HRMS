import { useAuthStore } from "../store/useAuthStore";
import { Navigate, Outlet } from "react-router-dom";

export function ProtectedRoute() {
    const isAuthenticated = useAuthStore(
        (state) => Boolean(state.accessToken)
    );

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }
    
    return <Outlet />
}