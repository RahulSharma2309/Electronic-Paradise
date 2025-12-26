import React from 'react';
import PropTypes from 'prop-types';
import '../../styles/components/common.css';

/**
 * Reusable Error Message component
 */
const ErrorMessage = ({ message, className = '' }) => {
  if (!message) return null;

  return <div className={`error-message ${className}`.trim()}>{message}</div>;
};

ErrorMessage.propTypes = {
  message: PropTypes.string,
  className: PropTypes.string,
};

export default ErrorMessage;

