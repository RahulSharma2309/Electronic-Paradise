import axios from "axios";
import { API_CONFIG, STORAGE_KEYS } from "../config/constants";

/**
 * Axios instance configured with base URL
 */
const api = axios.create({
  baseURL: API_CONFIG.BASE_URL,
  timeout: API_CONFIG.TIMEOUT,
});

/**
 * Request interceptor - adds JWT token to all requests
 */
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(STORAGE_KEYS.TOKEN);
    if (!token) {
      return config;
    }

    // Axios may initialize headers lazily; ensure it's always an object.
    config.headers = config.headers ?? {};
    config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

/**
 * Response interceptor - handles common errors
 */
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle common errors (401, 403, 500, etc.)
    if (error.response?.status === 401) {
      // Token expired or invalid - clear auth
      localStorage.removeItem(STORAGE_KEYS.TOKEN);
      localStorage.removeItem(STORAGE_KEYS.USER_ID);

      // Notify the app so hooks/state can update without a hard reload.
      // (Avoid forcing window.location.href which breaks refresh/deep-links.)
      window.dispatchEvent(new CustomEvent("ep:auth-unauthorized"));
    }
    return Promise.reject(error);
  }
);

export default api;

