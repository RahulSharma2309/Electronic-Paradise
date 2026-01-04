import { 
  validateEmail, 
  validatePhone, 
  validatePassword, 
  validatePasswordMatch, 
  validateRequired,
  validateRegistrationForm,
  validateLoginForm
} from '../../../utils/validations';

describe('Validation Utilities', () => {
  describe('validateEmail', () => {
    test('returns true for valid email formats', () => {
      expect(validateEmail('test@example.com')).toBe(true);
      expect(validateEmail('user.name@domain.co.uk')).toBe(true);
      expect(validateEmail('a@b.c')).toBe(true);
    });

    test('returns false for invalid email formats', () => {
      expect(validateEmail('')).toBe(false);
      expect(validateEmail('invalid-email')).toBe(false);
      expect(validateEmail('test@')).toBe(false);
      expect(validateEmail('@domain.com')).toBe(false);
      expect(validateEmail('test@domain')).toBe(false);
      expect(validateEmail(null)).toBe(false);
    });
  });

  describe('validatePhone', () => {
    test('returns true for valid phone numbers', () => {
      expect(validatePhone('1234567890')).toBe(true);
      expect(validatePhone('+123456789012')).toBe(true);
    });

    test('returns false for invalid phone numbers', () => {
      expect(validatePhone('123')).toBe(false); // Too short
      expect(validatePhone('1234567890123456')).toBe(false); // Too long
      expect(validatePhone('abc1234567')).toBe(false); // Contains letters
      expect(validatePhone('')).toBe(false);
    });
  });

  describe('validatePassword', () => {
    test('returns true for strong passwords', () => {
      expect(validatePassword('Password123!')).toBe(true);
    });

    test('returns false for weak passwords', () => {
      expect(validatePassword('short')).toBe(false); // Too short
      expect(validatePassword('password123!')).toBe(false); // No uppercase
      expect(validatePassword('PASSWORD123!')).toBe(false); // No lowercase
      expect(validatePassword('Password!')).toBe(false); // No number
      expect(validatePassword('Password123')).toBe(false); // No special char
    });
  });

  describe('validatePasswordMatch', () => {
    test('returns true when passwords match', () => {
      expect(validatePasswordMatch('password', 'password')).toBe(true);
    });

    test('returns false when passwords do not match', () => {
      expect(validatePasswordMatch('password', 'other')).toBe(false);
    });
  });

  describe('validateRequired', () => {
    test('returns true for non-empty values', () => {
      expect(validateRequired('content')).toBe(true);
      expect(validateRequired(123)).toBe(true);
    });

    test('returns false for empty values', () => {
      expect(validateRequired('')).toBe(false);
      expect(validateRequired('   ')).toBe(false);
      expect(validateRequired(null)).toBe(false);
      expect(validateRequired(undefined)).toBe(false);
    });
  });

  describe('validateRegistrationForm', () => {
    test('returns empty errors object for valid data', () => {
      const validData = {
        fullName: 'Test User',
        email: 'test@example.com',
        phone: '1234567890',
        password: 'Password123!',
        confirmPassword: 'Password123!'
      };
      expect(validateRegistrationForm(validData)).toEqual({});
    });

    test('returns error messages for invalid data', () => {
      const invalidData = {
        fullName: '',
        email: 'invalid',
        phone: '123',
        password: 'weak',
        confirmPassword: 'mismatch'
      };
      const errors = validateRegistrationForm(invalidData);
      expect(errors.fullName).toBeDefined();
      expect(errors.email).toBeDefined();
      expect(errors.phone).toBeDefined();
      expect(errors.password).toBeDefined();
      expect(errors.confirmPassword).toBeDefined();
    });
  });

  describe('validateLoginForm', () => {
    test('returns empty errors object for valid data', () => {
      const validData = {
        email: 'test@example.com',
        password: 'password'
      };
      expect(validateLoginForm(validData)).toEqual({});
    });

    test('returns errors for invalid data', () => {
      const invalidData = {
        email: '',
        password: ''
      };
      const errors = validateLoginForm(invalidData);
      expect(errors.email).toBeDefined();
      expect(errors.password).toBeDefined();
    });
  });
});



