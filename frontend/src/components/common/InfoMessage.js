import React from 'react';
import PropTypes from 'prop-types';
import '../../styles/components/auth.css';

/**
 * Reusable Info/Warning Message component
 */
const InfoMessage = ({ message, type = 'info', className = '' }) => {
  if (!message) return null;

  const messageClassName =
    type === 'success'
      ? `success-message ${className}`
      : `info-message ${className}`;

  return <div className={messageClassName.trim()}>{message}</div>;
};

InfoMessage.propTypes = {
  message: PropTypes.string,
  type: PropTypes.oneOf(['info', 'success']),
  className: PropTypes.string,
};

export default InfoMessage;

