import { useState, useCallback, useEffect } from "react";
import { STORAGE_KEYS } from "../config/constants";
import { authApi } from "../api/authApi";

/**
 * Custom hook for authentication state management
 */
export const useAuth = () => {
  const [token, setToken] = useState(() =>
    localStorage.getItem(STORAGE_KEYS.TOKEN)
  );
  const [userId, setUserId] = useState(() =>
    localStorage.getItem(STORAGE_KEYS.USER_ID)
  );
  const [isValidating, setIsValidating] = useState(true);

  // Allow the API client to signal that the token became invalid (401).
  useEffect(() => {
    const handleUnauthorized = () => {
      localStorage.removeItem(STORAGE_KEYS.TOKEN);
      localStorage.removeItem(STORAGE_KEYS.USER_ID);
      setToken(null);
      setUserId(null);
      setIsValidating(false);
    };

    window.addEventListener("ep:auth-unauthorized", handleUnauthorized);
    return () => {
      window.removeEventListener("ep:auth-unauthorized", handleUnauthorized);
    };
  }, []);

  // Validate token on mount
  useEffect(() => {
    const validateToken = async () => {
      const storedToken = localStorage.getItem(STORAGE_KEYS.TOKEN);
      if (!storedToken) {
        setIsValidating(false);
        return;
      }

      try {
        // Validate token by calling /api/auth/me endpoint
        const response = await authApi.getMe();
        // Token is valid, keep authentication state
        // Ensure userId is set if not already set
        if (response.data?.id) {
          const currentUserId = localStorage.getItem(STORAGE_KEYS.USER_ID);
          if (!currentUserId || currentUserId !== response.data.id.toString()) {
            localStorage.setItem(
              STORAGE_KEYS.USER_ID,
              response.data.id.toString()
            );
            setUserId(response.data.id.toString());
          }
        }
        setIsValidating(false);
      } catch (error) {
        const status = error?.response?.status;

        // Only clear auth on a real "unauthorized" response.
        // Network errors / transient backend issues should not force-log-out users on refresh.
        if (status === 401 || status === 403) {
          console.error("Token validation failed (unauthorized):", error);
          localStorage.removeItem(STORAGE_KEYS.TOKEN);
          localStorage.removeItem(STORAGE_KEYS.USER_ID);
          setToken(null);
          setUserId(null);
          setIsValidating(false);
          return;
        }

        console.error("Token validation failed (non-auth error):", error);
        setIsValidating(false);
      }
    };

    validateToken();
  }, []);

  const login = useCallback((tokenValue, userIdValue) => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, tokenValue);
    localStorage.setItem(STORAGE_KEYS.USER_ID, userIdValue);
    setToken(tokenValue);
    setUserId(userIdValue);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER_ID);
    setToken(null);
    setUserId(null);
  }, []);

  const isAuthenticated = !!token && !isValidating;

  return {
    token,
    userId,
    isAuthenticated,
    isValidating,
    login,
    logout,
  };
};
