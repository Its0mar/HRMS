import axios from "axios";
import { API_ROUTES } from "./apiRoutes";
import { useAuthStore } from "../store/useAuthStore";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const apiClient = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true,
});

const refreshClient = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true,
});

let refreshPromise: Promise<string> | null = null;

const refreshAccessToken = () => {
    if (!refreshPromise) {
        refreshPromise = refreshClient
            .post(API_ROUTES.AUTH.REFRESH, {})
            .then((response) => {
                const accessToken = response.data.accessToken;

                if (!accessToken) {
                    throw new Error("Refresh response did not include an access token.");
                }

                return accessToken as string;
            })
            .finally(() => {
                refreshPromise = null;
            });
    }

    return refreshPromise;
};

apiClient.interceptors.request.use((config) => {
    const accessToken = useAuthStore.getState().accessToken;

    if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
});

apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (!originalRequest) {
            return Promise.reject(error);
        }

        const requestUrl = originalRequest?.url ?? "";
        const isAuthEndpoint = [API_ROUTES.AUTH.LOGIN, API_ROUTES.AUTH.REGISTER, API_ROUTES.AUTH.REFRESH].some((route) =>
            requestUrl.includes(route)
        );

        if (error.response?.status === 401 && !originalRequest?._retry && !isAuthEndpoint) {
            originalRequest._retry = true;

            try {
                const newAccessToken = await refreshAccessToken();

                useAuthStore.getState().setAccessToken(newAccessToken);
                originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;

                return apiClient(originalRequest);
            } catch (refreshError) {
                useAuthStore.getState().clearSession();
                window.location.href = "/login";
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);
