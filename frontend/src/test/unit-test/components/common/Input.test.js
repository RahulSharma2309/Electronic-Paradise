import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Input from '../../../../components/common/Input';

describe('Input Component', () => {
  test('renders with placeholder and value', () => {
    render(<Input placeholder="Enter name" value="John" onChange={() => {}} />);
    const input = screen.getByPlaceholderText('Enter name');
    expect(input).toBeInTheDocument();
    expect(input.value).toBe('John');
  });

  test('calls onChange when text is entered', () => {
    const handleChange = jest.fn();
    render(<Input placeholder="Enter name" value="" onChange={handleChange} />);
    
    const input = screen.getByPlaceholderText('Enter name');
    fireEvent.change(input, { target: { value: 'Jane' } });
    
    expect(handleChange).toHaveBeenCalledTimes(1);
  });

  test('shows error message and applies error class', () => {
    render(<Input value="" onChange={() => {}} error="Required field" />);
    
    expect(screen.getByText('Required field')).toBeInTheDocument();
    const input = screen.getByRole('textbox');
    expect(input).toHaveClass('error');
  });
});

