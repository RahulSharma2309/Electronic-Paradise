/**
 * API Endpoints configuration
 * Centralized location for all API endpoint paths
 */

const BASE_PATH = '/api';

export const API_ENDPOINTS = {
  // Authentication endpoints
  AUTH: {
    REGISTER: `${BASE_PATH}/auth/register`,
    LOGIN: `${BASE_PATH}/auth/login`,
    RESET_PASSWORD: `${BASE_PATH}/auth/reset-password`,
    ME: `${BASE_PATH}/auth/me`,
  },

  // User endpoints
  USERS: {
    BASE: `${BASE_PATH}/users`,
    BY_ID: (id) => `${BASE_PATH}/users/${id}`,
    BY_USER_ID: (userId) => `${BASE_PATH}/users/by-userid/${encodeURIComponent(userId)}`,
    PHONE_EXISTS: (phoneNumber) =>
      `${BASE_PATH}/users/phone-exists/${encodeURIComponent(phoneNumber)}`,
    WALLET_DEBIT: (id) => `${BASE_PATH}/users/${id}/wallet/debit`,
    WALLET_CREDIT: (id) => `${BASE_PATH}/users/${id}/wallet/credit`,
    ADD_BALANCE: `${BASE_PATH}/users/add-balance`,
  },

  // Product endpoints
  PRODUCTS: {
    BASE: `${BASE_PATH}/products`,
    BY_ID: (id) => `${BASE_PATH}/products/${id}`,
    RESERVE: (id) => `${BASE_PATH}/products/${id}/reserve`,
    RELEASE: (id) => `${BASE_PATH}/products/${id}/release`,
  },

  // Order endpoints
  ORDERS: {
    CREATE: `${BASE_PATH}/orders/create`,
    BY_ID: (id) => `${BASE_PATH}/orders/${id}`,
    BY_USER: (userId) => `${BASE_PATH}/orders/user/${userId}`,
  },

  // Payment endpoints
  PAYMENTS: {
    RECORD: `${BASE_PATH}/payments/record`,
    STATUS: (orderId) => `${BASE_PATH}/payments/status/${orderId}`,
  },

  // Health check
  HEALTH: {
    BASE: `${BASE_PATH}/health`,
  },
};

