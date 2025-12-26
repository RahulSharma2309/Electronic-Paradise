import { useState, useCallback, useEffect } from 'react';
import { STORAGE_KEYS } from '../config/constants';
import { authApi } from '../api/authApi';

/**
 * Custom hook for authentication state management
 */
export const useAuth = () => {
  const [token, setToken] = useState(() => localStorage.getItem(STORAGE_KEYS.TOKEN));
  const [userId, setUserId] = useState(() => localStorage.getItem(STORAGE_KEYS.USER_ID));
  const [isValidating, setIsValidating] = useState(true);

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
            localStorage.setItem(STORAGE_KEYS.USER_ID, response.data.id.toString());
            setUserId(response.data.id.toString());
          }
        }
        setIsValidating(false);
      } catch (error) {
        // Token is invalid or expired, clear auth
        console.error('Token validation failed:', error);
        localStorage.removeItem(STORAGE_KEYS.TOKEN);
        localStorage.removeItem(STORAGE_KEYS.USER_ID);
        setToken(null);
        setUserId(null);
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

