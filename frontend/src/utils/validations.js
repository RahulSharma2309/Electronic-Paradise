/**
 * Validation utility functions
 * All validation functions return boolean or validation result
 */

/**
 * Validates email format
 * @param {string} email - Email address to validate
 * @returns {boolean} - True if email format is valid
 */
export const validateEmail = (email) => {
  if (!email || typeof email !== 'string') return false;
  return /^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email.trim());
};

/**
 * Validates phone number format (10-15 digits, optional + prefix)
 * @param {string} phone - Phone number to validate
 * @returns {boolean} - True if phone format is valid
 */
export const validatePhone = (phone) => {
  if (!phone || typeof phone !== 'string') return false;
  return /^\+?\d{10,15}$/.test(phone.trim());
};

/**
 * Validates password strength
 * Requirements: 8+ chars, at least 1 uppercase, 1 lowercase, 1 number, 1 special character
 * @param {string} password - Password to validate
 * @returns {boolean} - True if password meets strength requirements
 */
export const validatePassword = (password) => {
  if (!password || typeof password !== 'string') return false;
  return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/.test(password);
};

/**
 * Validates that password and confirm password match
 * @param {string} password - Original password
 * @param {string} confirmPassword - Confirmation password
 * @returns {boolean} - True if passwords match
 */
export const validatePasswordMatch = (password, confirmPassword) => {
  if (!password || !confirmPassword) return false;
  return password === confirmPassword;
};

/**
 * Validates that a required field is not empty
 * @param {string} value - Value to check
 * @returns {boolean} - True if value is not empty
 */
export const validateRequired = (value) => {
  if (typeof value === 'string') {
    return value.trim().length > 0;
  }
  return value != null && value !== '';
};

/**
 * Registration form validation
 * @param {Object} formData - Form data object
 * @param {string} formData.fullName - Full name
 * @param {string} formData.email - Email address
 * @param {string} formData.phone - Phone number
 * @param {string} formData.password - Password
 * @param {string} formData.confirmPassword - Confirm password
 * @returns {Object} - Object with field names as keys and error messages as values
 */
export const validateRegistrationForm = (formData) => {
  const errors = {};

  // Full Name validation
  if (!validateRequired(formData.fullName)) {
    errors.fullName = 'Full name is required';
  }

  // Email validation
  if (!validateRequired(formData.email)) {
    errors.email = 'Email is required';
  } else if (!validateEmail(formData.email)) {
    errors.email = 'Invalid email format';
  }

  // Phone validation
  if (!validateRequired(formData.phone)) {
    errors.phone = 'Phone number is required';
  } else if (!validatePhone(formData.phone)) {
    errors.phone = 'Invalid phone number format (10-15 digits, optional + prefix)';
  }

  // Password validation
  if (!validateRequired(formData.password)) {
    errors.password = 'Password is required';
  } else if (!validatePassword(formData.password)) {
    errors.password =
      'Password must be 8+ characters, include uppercase, lowercase, number, and special character';
  }

  // Confirm Password validation
  if (!validateRequired(formData.confirmPassword)) {
    errors.confirmPassword = 'Please confirm your password';
  } else if (!validatePasswordMatch(formData.password, formData.confirmPassword)) {
    errors.confirmPassword = 'Passwords do not match';
  }

  return errors;
};

/**
 * Login form validation
 * @param {Object} formData - Form data object
 * @param {string} formData.email - Email address
 * @param {string} formData.password - Password
 * @returns {Object} - Object with field names as keys and error messages as values
 */
export const validateLoginForm = (formData) => {
  const errors = {};

  if (!validateRequired(formData.email)) {
    errors.email = 'Email is required';
  } else if (!validateEmail(formData.email)) {
    errors.email = 'Invalid email format';
  }

  if (!validateRequired(formData.password)) {
    errors.password = 'Password is required';
  }

  return errors;
};

