import { create } from 'zustand';
import { persist } from "zustand/middleware";

export interface UserInfo {
    id: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    organizationId: number;
    permissions : string[];
}

interface AuthState {
    user: UserInfo | null;
    accessToken: string | null;

    setSession: (user: UserInfo, accessToken: string) => void;
    setAccessToken: (accessToken: string) => void;
    clearSession: () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            user: null,
            accessToken: null,

            setSession: (user, accessToken) =>
                set({ user, accessToken }),

            setAccessToken: (accessToken) =>
                set({ accessToken }),

            clearSession: () => {
                localStorage.removeItem("hrms-employee-options");

                set({
                    user: null,
                    accessToken: null
                });
            }
        }),
        {
            name: "hrms-auth"
        }
    )
);
