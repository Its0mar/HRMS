import { useAuthStore } from "../../../store/useAuthStore";

export function BasicInfo() {
    const user = useAuthStore((state) => state.user);
    return (
        <div>
            <h1>Dashboard</h1>
            <div>
                <p>Hello :</p>
                <p>{user?.firstName} + {user?.lastName}</p>
            </div>

            <div>
                <p>email: {user?.email}</p>
            </div>

        </div>
    );
}