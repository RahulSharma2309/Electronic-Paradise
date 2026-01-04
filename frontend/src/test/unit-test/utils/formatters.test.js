import { 
  formatCurrency, 
  formatINR, 
  splitFullName, 
  formatErrorMessage,
  calculateCartTotal,
  calculateCartQuantity
} from '../../../utils/formatters';

describe('Formatter Utilities', () => {
  describe('formatCurrency', () => {
    test('formats amount correctly with symbol', () => {
      expect(formatCurrency(100)).toBe('₹100.00');
      expect(formatCurrency(1234.567)).toBe('₹1234.57');
    });

    test('formats amount correctly without symbol', () => {
      expect(formatCurrency(100, 1, 2, false)).toBe('100.00');
    });

    test('handles null or invalid amount', () => {
      expect(formatCurrency(null)).toBe('₹0.00');
      expect(formatCurrency(undefined)).toBe('₹0.00');
      expect(formatCurrency('abc')).toBe('₹0.00');
    });
  });

  describe('formatINR', () => {
    test('formats INR correctly', () => {
      expect(formatINR(500)).toBe('₹500.00');
    });
  });

  describe('splitFullName', () => {
    test('splits first and last name correctly', () => {
      expect(splitFullName('John Doe')).toEqual({ firstName: 'John', lastName: 'Doe' });
      expect(splitFullName('John Michael Doe')).toEqual({ firstName: 'John', lastName: 'Michael Doe' });
    });

    test('handles single name', () => {
      expect(splitFullName('John')).toEqual({ firstName: 'John', lastName: '' });
    });

    test('handles empty or null name', () => {
      expect(splitFullName('')).toEqual({ firstName: '', lastName: '' });
      expect(splitFullName(null)).toEqual({ firstName: '', lastName: '' });
    });
  });

  describe('formatErrorMessage', () => {
    test('extracts error from response data', () => {
      const error = { response: { data: { error: 'API Error' } } };
      expect(formatErrorMessage(error)).toBe('API Error');
    });

    test('extracts message from error object', () => {
      const error = { message: 'Network Error' };
      expect(formatErrorMessage(error)).toBe('Network Error');
    });

    test('returns default message if no info found', () => {
      expect(formatErrorMessage({}, 'Default')).toBe('Default');
    });
  });

  describe('calculateCartTotal', () => {
    test('calculates total correctly', () => {
      const items = [
        { price: 100, quantity: 2 },
        { price: 50, quantity: 1 }
      ];
      expect(calculateCartTotal(items)).toBe(250);
    });

    test('handles empty or null items', () => {
      expect(calculateCartTotal([])).toBe(0);
      expect(calculateCartTotal(null)).toBe(0);
    });
  });

  describe('calculateCartQuantity', () => {
    test('calculates total quantity correctly', () => {
      const items = [
        { quantity: 2 },
        { quantity: 3 }
      ];
      expect(calculateCartQuantity(items)).toBe(5);
    });
  });
});



