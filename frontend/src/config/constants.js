/**
 * Application-wide constants
 */

// API Configuration
export const API_CONFIG = {
  BASE_URL: process.env.REACT_APP_API_BASE || 'http://localhost:5000',
  TIMEOUT: 30000, // 30 seconds
};

// Local Storage Keys
export const STORAGE_KEYS = {
  TOKEN: 'token',
  USER_ID: 'userId',
  WISHLIST: 'wishlist',
};

// Route Paths
export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/register',
  PRODUCTS: '/products',
  CART: '/cart',
  CHECKOUT: '/checkout',
  PROFILE: '/profile',
  ORDERS: '/orders',
  WISHLIST: '/wishlist',
};

// Error Messages
export const ERROR_MESSAGES = {
  GENERIC: 'An error occurred. Please try again.',
  NETWORK: 'Network error. Please check your connection.',
  UNAUTHORIZED: 'You are not authorized to perform this action.',
  NOT_FOUND: 'Resource not found.',
  REGISTRATION_FAILED: 'Registration failed. Please try again.',
  LOGIN_FAILED: 'Login failed. Please check your credentials.',
};

// Success Messages
export const SUCCESS_MESSAGES = {
  REGISTERED: 'Registered and logged in successfully',
  LOGGED_IN: 'Logged in successfully',
  LOGGED_OUT: 'Logged out successfully',
  ORDER_PLACED: 'Order placed successfully!',
};

// Currency Configuration (INR)
export const CURRENCY_CONFIG = {
  SYMBOL: '₹',
  CODE: 'INR',
  DIVISOR: 1, // INR stored as whole rupees (not paise)
  DECIMAL_PLACES: 2,
};

// Wallet Display (deprecated - use CURRENCY_CONFIG)
export const WALLET_CONFIG = {
  DIVISOR: 1, // INR stored as whole rupees
  DECIMAL_PLACES: 2,
};

