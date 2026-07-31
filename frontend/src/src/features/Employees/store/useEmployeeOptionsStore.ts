import { create } from "zustand";
import { persist } from "zustand/middleware";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { useAuthStore } from "../../../store/useAuthStore";
import type { EmployeeOption } from "../types/EmployeeOption";


interface EmployeeOptionsState {
    employees: EmployeeOption[];
    isLoading: boolean;
    error: string | null;
    loadedAt: number | null;
    organizationId: number | null;

    loadEmployees: (force?: boolean) => Promise<void>;
    invalidate: () => void;
}

const CACHE_DURATION = 5 * 60 * 1000;

export const useEmployeeOptionsStore =
    create<EmployeeOptionsState>()(
        persist(
            (set, get) => ({
                employees: [],
                isLoading: false,
                error: null,
                loadedAt: null,
                organizationId: null,

                loadEmployees: async (force = false) => {
                    const currentOrganizationId =
                        useAuthStore.getState().user?.organizationId ?? null;
                    const { isLoading, loadedAt, organizationId } = get();

                    if (currentOrganizationId === null) {
                        set({
                            employees: [],
                            loadedAt: null,
                            organizationId: null
                        });
                        return;
                    }

                    const cacheIsValid =
                        organizationId === currentOrganizationId &&
                        loadedAt !== null &&
                        Date.now() - loadedAt < CACHE_DURATION;

                    if (isLoading || (!force && cacheIsValid)) {
                        return;
                    }

                    set({ isLoading: true, error: null });

                    try {
                        const response =
                            await apiClient.get<EmployeeOption[]>(
                                API_ROUTES.EMPLOYEES.GET_OPTIONS
                            );

                        set({
                            employees: response.data,
                            loadedAt: Date.now(),
                            organizationId: currentOrganizationId
                        });
                    } catch {
                        set({
                            employees: [],
                            loadedAt: null,
                            error: "Could not load employees."
                        });
                    } finally {
                        set({ isLoading: false });
                    }
                },

                invalidate: () => {
                    set({
                        employees: [],
                        loadedAt: null,
                        organizationId: null,
                        error: null
                    });
                }
            }),
            {
                name: "hrms-employee-options",

                // Do not persist temporary loading state.
                partialize: (state) => ({
                    employees: state.employees,
                    loadedAt: state.loadedAt,
                    organizationId: state.organizationId
                })
            }
        )
    );
