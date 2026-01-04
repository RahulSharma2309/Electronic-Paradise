import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Button from '../../../../components/common/Button';

describe('Button Component', () => {
  test('renders with children', () => {
    render(<Button>Click Me</Button>);
    expect(screen.getByText('Click Me')).toBeInTheDocument();
  });

  test('calls onClick when clicked', () => {
    const handleClick = jest.fn();
    render(<Button onClick={handleClick}>Click Me</Button>);
    
    fireEvent.click(screen.getByText('Click Me'));
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  test('is disabled when disabled prop is true', () => {
    render(<Button disabled>Click Me</Button>);
    expect(screen.getByText('Click Me')).toBeDisabled();
  });

  test('is disabled and shows spinner when loading prop is true', () => {
    render(<Button loading>Click Me</Button>);
    expect(screen.getByText('Click Me')).toBeDisabled();
    expect(document.querySelector('.loading-spinner')).toBeInTheDocument();
  });

  test('applies variant class correctly', () => {
    const { container } = render(<Button variant="secondary">Click Me</Button>);
    expect(container.firstChild).toHaveClass('button-secondary');
  });
});

