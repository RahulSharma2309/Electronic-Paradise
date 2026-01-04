import { renderHook, act } from '@testing-library/react';
import { useAuth } from '../../../hooks/useAuth';
import { STORAGE_KEYS } from '../../../config/constants';
import { authApi } from '../../../api/authApi';

// Mock authApi
jest.mock('../../../api/authApi', () => ({
  authApi: {
    getMe: jest.fn(),
  },
}));

describe('useAuth Hook', () => {
  beforeEach(() => {
    localStorage.clear();
    jest.clearAllMocks();
  });

  test('initializes with null when localStorage is empty', () => {
    const { result } = renderHook(() => useAuth());
    
    expect(result.current.token).toBeNull();
    expect(result.current.userId).toBeNull();
    expect(result.current.isAuthenticated).toBe(false);
  });

  test('initializes with values from localStorage', () => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, 'test-token');
    localStorage.setItem(STORAGE_KEYS.USER_ID, 'user-123');
    
    const { result } = renderHook(() => useAuth());
    
    expect(result.current.token).toBe('test-token');
    expect(result.current.userId).toBe('user-123');
  });

  test('login sets token and userId in localStorage and state', () => {
    const { result } = renderHook(() => useAuth());
    
    act(() => {
      result.current.login('new-token', 'new-user');
    });
    
    expect(localStorage.getItem(STORAGE_KEYS.TOKEN)).toBe('new-token');
    expect(localStorage.getItem(STORAGE_KEYS.USER_ID)).toBe('new-user');
    expect(result.current.token).toBe('new-token');
    expect(result.current.userId).toBe('new-user');
  });

  test('logout removes values from localStorage and state', () => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, 'test-token');
    localStorage.setItem(STORAGE_KEYS.USER_ID, 'user-123');
    
    const { result } = renderHook(() => useAuth());
    
    act(() => {
      result.current.logout();
    });
    
    expect(localStorage.getItem(STORAGE_KEYS.TOKEN)).toBeNull();
    expect(localStorage.getItem(STORAGE_KEYS.USER_ID)).toBeNull();
    expect(result.current.token).toBeNull();
    expect(result.current.userId).toBeNull();
  });

  test('validates token on mount and keeps it if valid', async () => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, 'valid-token');
    authApi.getMe.mockResolvedValue({ data: { id: 'user-123' } });
    
    const { result } = renderHook(() => useAuth());
    
    // Wait for validation to finish
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 0));
    });
    
    expect(result.current.isValidating).toBe(false);
    expect(result.current.token).toBe('valid-token');
    expect(result.current.isAuthenticated).toBe(true);
  });

  test('validates token on mount and clears it if invalid', async () => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, 'invalid-token');
    authApi.getMe.mockRejectedValue(new Error('Unauthorized'));
    
    const { result } = renderHook(() => useAuth());
    
    // Wait for validation to finish
    await act(async () => {
      await new Promise(resolve => setTimeout(resolve, 0));
    });
    
    expect(result.current.isValidating).toBe(false);
    expect(result.current.token).toBeNull();
    expect(result.current.isAuthenticated).toBe(false);
    expect(localStorage.getItem(STORAGE_KEYS.TOKEN)).toBeNull();
  });
});

