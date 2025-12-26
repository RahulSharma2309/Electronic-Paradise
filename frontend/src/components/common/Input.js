import React from 'react';
import PropTypes from 'prop-types';
import '../../styles/components/common.css';

/**
 * Reusable Input component
 */
const Input = ({
  type = 'text',
  placeholder,
  value,
  onChange,
  error,
  className = '',
  ...rest
}) => {
  const inputClassName = `input ${error ? 'error' : ''} ${className}`.trim();

  return (
    <div className="form-group">
      <input
        type={type}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        className={inputClassName}
        {...rest}
      />
      {error && <div className="error-message">{error}</div>}
    </div>
  );
};

Input.propTypes = {
  type: PropTypes.string,
  placeholder: PropTypes.string,
  value: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
  error: PropTypes.string,
  className: PropTypes.string,
};

export default Input;

