/**
 * Formatting utility functions
 */

/**
 * Formats amount as Indian Rupees (INR)
 * @param {number} amount - Amount in rupees
 * @param {number} divisor - Divisor to convert (default: 1 for INR)
 * @param {number} decimalPlaces - Number of decimal places (default: 2)
 * @param {boolean} includeSymbol - Whether to include currency symbol (default: true)
 * @returns {string} - Formatted currency string with ₹ symbol
 */
export const formatCurrency = (
  amount,
  divisor = 1,
  decimalPlaces = 2,
  includeSymbol = true
) => {
  if (amount == null || isNaN(amount)) {
    return includeSymbol ? "₹0.00" : "0.00";
  }
  const rupees = amount / divisor;
  const formatted = rupees.toFixed(decimalPlaces);
  return includeSymbol ? `₹${formatted}` : formatted;
};

/**
 * Formats INR currency with proper formatting
 * @param {number} amount - Amount in rupees
 * @returns {string} - Formatted INR string
 */
export const formatINR = (amount) => {
  return formatCurrency(amount, 1, 2, true);
};

/**
 * Splits full name into first name and last name
 * @param {string} fullName - Full name string
 * @returns {Object} - Object with firstName and lastName
 */
export const splitFullName = (fullName) => {
  if (!fullName || typeof fullName !== "string") {
    return { firstName: "", lastName: "" };
  }

  const names = fullName.trim().split(/\s+/).filter(Boolean);

  if (names.length === 0) {
    return { firstName: "", lastName: "" };
  }

  if (names.length === 1) {
    return { firstName: names[0], lastName: "" };
  }

  return {
    firstName: names[0],
    lastName: names.slice(1).join(" "),
  };
};

/**
 * Formats error message from API response
 * @param {Error} error - Axios error object
 * @param {string} defaultMessage - Default message if error format is unknown
 * @returns {string} - Formatted error message
 */
export const formatErrorMessage = (
  error,
  defaultMessage = "An error occurred"
) => {
  if (!error) return defaultMessage;

  if (error.response?.data?.error) {
    return error.response.data.error;
  }

  if (error.message) {
    return error.message;
  }

  return defaultMessage;
};

/**
 * Calculates total price from cart items
 * @param {Array} cartItems - Array of cart items with price and quantity
 * @returns {number} - Total price
 */
export const calculateCartTotal = (cartItems) => {
  if (!Array.isArray(cartItems)) return 0;

  return cartItems.reduce((total, item) => {
    const price = item.price || 0;
    const quantity = item.quantity || 0;
    return total + price * quantity;
  }, 0);
};

/**
 * Calculates total quantity of items in cart
 * @param {Array} cartItems - Array of cart items with quantity
 * @returns {number} - Total quantity
 */
export const calculateCartQuantity = (cartItems) => {
  if (!Array.isArray(cartItems)) return 0;

  return cartItems.reduce((total, item) => {
    return total + (item.quantity || 0);
  }, 0);
};
