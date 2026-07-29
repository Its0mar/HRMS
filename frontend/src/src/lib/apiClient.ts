import axios from "axios";
import { API_ROUTES } from "./apiRoutes";
import { useAuthStore } from "../store/useAuthStore";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const getStoredToken = () => localStorage.getItem("token") ?? localStorage.getItem("accessToken");

export const apiClient = axios.create({
    baseURL: API_BASE_URL,
    withCredentials : true,
});

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
        const requestUrl = originalRequest?.url ?? "";
        const isAuthEndpoint = [API_ROUTES.AUTH.LOGIN, API_ROUTES.AUTH.REGISTER, API_ROUTES.AUTH.REFRESH].some((route) =>
            requestUrl.includes(route)
        );

        if (error.response?.status === 401 && !originalRequest?._retry && !isAuthEndpoint) {
            originalRequest._retry = true;
            try {
                const response = await apiClient.post(API_ROUTES.AUTH.REFRESH, {});

                const newAccessToken = response.data.accessToken ?? response.data.token;
                if (newAccessToken) {
                    useAuthStore.getState().setAccessToken(newAccessToken);
                    originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
                    window.location.reload();
                }

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