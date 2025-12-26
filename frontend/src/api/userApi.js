import api from './index';
import { API_ENDPOINTS } from '../config/apiEndpoints';

/**
 * User API service functions
 */
export const userApi = {
  /**
   * Create user profile
   */
  createProfile: (profile) => {
    return api.post(API_ENDPOINTS.USERS.BASE, profile);
  },

  /**
   * Get user profile by User ID (from auth service)
   */
  getProfileByUserId: (userId) => {
    return api.get(API_ENDPOINTS.USERS.BY_USER_ID(userId));
  },

  /**
   * Get user profile by profile ID
   */
  getProfileById: (id) => {
    return api.get(API_ENDPOINTS.USERS.BY_ID(id));
  },

  /**
   * Update user profile
   */
  updateProfile: (id, profile) => {
    return api.put(API_ENDPOINTS.USERS.BY_ID(id), profile);
  },

  /**
   * Check if phone number exists
   */
  checkPhoneExists: (phoneNumber) => {
    return api.get(API_ENDPOINTS.USERS.PHONE_EXISTS(phoneNumber));
  },

  /**
   * Debit wallet
   */
  debitWallet: (id, amount) => {
    return api.post(API_ENDPOINTS.USERS.WALLET_DEBIT(id), { amount });
  },

  /**
   * Credit wallet
   */
  creditWallet: (id, amount) => {
    return api.post(API_ENDPOINTS.USERS.WALLET_CREDIT(id), { amount });
  },

  /**
   * Add balance to wallet using userId (Guid as string)
   */
  addBalance: (userId, amount) => {
    return api.post(API_ENDPOINTS.USERS.ADD_BALANCE, { userId, amount });
  },
};

