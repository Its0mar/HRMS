import { useAuthStore } from "../../../store/useAuthStore";

export function usePermission(permission: string) : boolean {
    const user = useAuthStore((state) => state.user);
    return user?.permissions.includes(permission) ?? false;
}