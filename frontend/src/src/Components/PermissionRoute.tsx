import { Navigate } from "react-router-dom";
import { usePermission } from "../features/Auth/hooks/usePermission";

interface PermissionRouteProps {
  permission: string;
  children: React.ReactNode;
}

export function PermissionRoute({
  permission,
  children,
}: PermissionRouteProps) {
  const allowed = usePermission(permission);

  if (!allowed) {
    return <Navigate to="/forbidden" replace />;
  }

  return children;
}