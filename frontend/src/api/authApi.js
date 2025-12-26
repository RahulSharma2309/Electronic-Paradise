import api from './index';
import { API_ENDPOINTS } from '../config/apiEndpoints';

/**
 * Authentication API service functions
 */
export const authApi = {
  /**
   * Register new user
   */
  register: (registerData) => {
    return api.post(API_ENDPOINTS.AUTH.REGISTER, registerData);
  },

  /**
   * Login user
   */
  login: (credentials) => {
    return api.post(API_ENDPOINTS.AUTH.LOGIN, {
      Email: credentials.email,
      Password: credentials.password,
    });
  },

  /**
   * Reset password
   */
  resetPassword: (resetData) => {
    return api.post(API_ENDPOINTS.AUTH.RESET_PASSWORD, resetData);
  },

  /**
   * Get current user info (requires authentication)
   * Used for token validation
   */
  getMe: () => {
    return api.get(API_ENDPOINTS.AUTH.ME);
  },

  /**
   * Validate token by checking if user exists
   */
  validateToken: () => {
    return api.get(API_ENDPOINTS.AUTH.ME);
  },
};

